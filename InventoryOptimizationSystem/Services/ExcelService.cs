using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using ProductInventoryOptimizer.Models;

namespace ProductInventoryOptimizer.Services
{
    /// <summary>
    /// Excel导入导出服务 - 使用 EPPlus 4.5.3（适用于 .NET Framework 4.8）
    /// </summary>
    public class ExcelService
    {
        public ExcelService()
        {
            // EPPlus 许可证上下文（非商业用途）
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        /// <summary>
        /// 从Excel导入产品和月度数据
        /// </summary>
        public (List<Product> products, List<MonthlyData> monthlyData, string message) ImportFromExcel(string filePath)
        {
            var products = new List<Product>();
            var monthlyData = new List<MonthlyData>();
            var message = string.Empty;

            try
            {
                if (!File.Exists(filePath))
                {
                    return (products, monthlyData, "文件不存在");
                }

                using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorkbook workbook = package.Workbook;

                    // 1. 读取产品信息（假设在第一个工作表）
                    if (workbook.Worksheets.Count > 0)
                    {
                        ExcelWorksheetWorksheet productSheet = workbook.Worksheets[1];
                        products = ParseProductSheet(productSheet);
                        message += "从工作表 '" + productSheet.Name + "' 导入 " + products.Count + " 个产品。";
                    }

                    // 2. 读取月度数据（假设在第二个工作表）
                    if (workbook.Worksheets.Count > 1)
                    {
                        ExcelWorksheetWorksheet monthlySheet = workbook.Worksheets[2];
                        monthlyData = ParseMonthlyDataSheet(monthlySheet);
                        message += "\n从工作表 '" + monthlySheet.Name + "' 导入 " + monthlyData.Count + " 条月度数据。";
                    }
                }

                return (products, monthlyData, message);
            }
            catch (Exception ex)
            {
                return (products, monthlyData, "导入失败: " + ex.Message);
            }
        }

        /// <summary>
        /// 解析产品工作表
        /// </summary>
        private List<Product> ParseProductSheet(ExcelWorksheetWorksheet sheet)
        {
            var products = new List<Product>();

            // 假设第一行为表头，从第二行开始读取数据
            int rowCount = sheet.Dimension != null ? sheet.Dimension.Rows : 0;

            for (int row = 2; row <= rowCount; row++)
            {
                string code = sheet.Cells[row, 1].Text?.Trim() ?? "";
                string name = sheet.Cells[row, 2].Text?.Trim() ?? "";
                string priceText = sheet.Cells[row, 3].Text?.Trim() ?? "0";
                string description = sheet.Cells[row, 4].Text?.Trim();

                // 跳过空行
                if (string.IsNullOrEmpty(code) && string.IsNullOrEmpty(name))
                    continue;

                decimal price = 0;
                decimal.TryParse(priceText, out price);

                products.Add(new Product
                {
                    Id = products.Count + 1,
                    Code = code,
                    Name = string.IsNullOrEmpty(name) ? code : name,
                    UnitPrice = price,
                    Description = description
                });
            }

            return products;
        }

        /// <summary>
        /// 解析月度数据工作表
        /// </summary>
        private List<MonthlyData> ParseMonthlyDataSheet(ExcelWorksheetWorksheet sheet)
        {
            var monthlyData = new List<MonthlyData>();

            int rowCount = sheet.Dimension != null ? sheet.Dimension.Rows : 0;

            for (int row = 2; row <= rowCount; row++)
            {
                string code = sheet.Cells[row, 1].Text?.Trim() ?? "";
                string yearText = sheet.Cells[row, 2].Text?.Trim() ?? "0";
                string monthText = sheet.Cells[row, 3].Text?.Trim() ?? "0";
                string demandText = sheet.Cells[row, 4].Text?.Trim() ?? "0";
                string targetStockText = sheet.Cells[row, 5].Text?.Trim() ?? "0";
                string startStockText = sheet.Cells[row, 6].Text?.Trim() ?? "0";

                if (string.IsNullOrEmpty(code))
                    continue;

                int year = 0, month = 0, demand = 0, targetStock = 0, startStock = 0;
                int.TryParse(yearText, out year);
                int.TryParse(monthText, out month);
                int.TryParse(demandText, out demand);
                int.TryParse(targetStockText, out targetStock);
                int.TryParse(startStockText, out startStock);

                monthlyData.Add(new MonthlyData
                {
                    ProductId = 0, // 需要根据产品编码匹配
                    Year = year,
                    Month = month,
                    MonthlyDemand = demand,
                    MonthEndTargetStock = targetStock,
                    MonthStartStock = startStock
                });
            }

            return monthlyData;
        }

        /// <summary>
        /// 导出优化结果到Excel
        /// </summary>
        public (bool success, string message) ExportResultToExcel(
            OptimizationResult result,
            List<Product> products,
            List<MonthlyData> monthlyData,
            string filePath)
        {
            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorkbook workbook = package.Workbook;

                    // 1. 汇总信息工作表
                    ExcelWorksheetWorksheet summarySheet = workbook.Worksheets.Add("汇总信息");
                    WriteSummarySheet(summarySheet, result);

                    // 2. 产品明细工作表
                    ExcelWorksheetWorksheet detailSheet = workbook.Worksheets.Add("产品明细");
                    WriteProductDetailSheet(detailSheet, result, products, monthlyData);

                    // 3. 原始数据工作表
                    ExcelWorksheetWorksheet rawSheet = workbook.Workbook.Add("原始数据");
                    WriteRawDataSheet(rawSheet, products, monthlyData);

                    package.SaveAs(new FileInfo(filePath));
                }

                return (true, "导出成功: " + filePath);
            }
            catch (Exception ex)
            {
                return (false, "导出失败: " + ex.Message);
            }
        }

        /// <summary>
        /// 写入汇总信息
        /// </summary>
        private void WriteSummarySheet(ExcelWorksheetWorksheet sheet, OptimizationResult result)
        {
            sheet.Cells[1, 1].Value = "汇总信息";
            sheet.Cells[1, 1].Style.Font.Bold = true;
            sheet.Cells[1, 1].Style.Font.Size = 14;

            int row = 3;

            sheet.Cells[row, 1].Value = "计算时间";
            sheet.Cells[row, 2].Value = result.ComputedAt.ToString("yyyy-MM-dd HH:mm:ss");
            row++;

            sheet.Cells[row, 1].Value = "求解状态";
            sheet.Cells[row, 2].Value = result.Status;
            sheet.Cells[row, 2].Style.Font.Bold = result.Success;
            row++;

            sheet.Cells[row, 1].Value = "计算耗时";
            sheet.Cells[row, 2].Value = result.ComputationTimeMs + " 毫秒";
            row++;

            row++;

            sheet.Cells[row, 1].Value = "目标销售额";
            sheet.Cells[row, 2].Value = result.TargetSalesAmount.ToString("C2");
            row++;

            sheet.Cells[row, 1].Value = "实际销售额";
            sheet.Cells[row, 2].Value = result.CalculatedSalesAmount.ToString("C2");
            row++;

            sheet.Cells[row, 1].Value = "销售额差额";
            sheet.Cells[row, 2].Value = result.SalesGap.ToString("C2");
            sheet.Cells[row, 2].Style.Font.Bold = result.SalesGap != 0;
            row++;

            row++;

            sheet.Cells[row, 1].Value = "最大供应量";
            sheet.Cells[row, 2].Value = result.MaxTotalSupply;
            row++;

            sheet.Cells[row, 1].Value = "实际供应量";
            sheet.Cells[row, 2].Value = result.ActualTotalSupply;
            row++;

            sheet.Cells[row, 1].Value = "供应量使用率";
            sheet.Cells[row, 2].Value = result.MaxTotalSupply > 0
                ? Math.Round(result.ActualTotalSupply * 100.0 / result.MaxTotalSupply, 2) + "%"
                : "N/A";

            sheet.Column(1).Width = 20;
            sheet.Column(2).Width = 25;
        }

        /// <summary>
        /// 写入产品明细
        /// </summary>
        private void WriteProductDetailSheet(
            ExcelWorksheetWorksheet sheet,
            OptimizationResult result,
            List<Product> products,
            List<MonthlyData> monthlyData)
        {
            sheet.Cells[1, 1].Value = "产品编码";
            sheet.Cells[1, 2].Value = "产品名称";
            sheet.Cells[1, 3].Value = "单价(元)";
            sheet.Cells[1, 4].Value = "月度需求";
            sheet.Cells[1, 5].Value = "月初库存";
            sheet.Cells[1, 6].Value = "月末目标库存";
            sheet.Cells[1, 7].Value = "建议销量";
            sheet.Cells[1, 8].Value = "预计销售额";
            sheet.Cells[1, 9].Value = "需求满足";
            sheet.Cells[1, 10].Value = "库存满足";

            for (int col = 1; col <= 10; col++)
            {
                sheet.Cells[1, col].Style.Font.Bold = true;
                sheet.Cells[1, col].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                sheet.Cells[1, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            }

            int row = 2;
            foreach (ProductOptimizationDetail detail in result.ProductDetails)
            {
                sheet.Cells[row, 1].Value = detail.ProductCode;
                sheet.Cells[row, 2].Value = detail.ProductName;
                sheet.Cells[row, 3].Value = detail.UnitPrice;
                sheet.Cells[row, 4].Value = detail.MonthlyDemand;
                sheet.Cells[row, 5].Value = detail.MonthStartStock;
                sheet.Cells[row, 6].Value = detail.MonthEndTargetStock;
                sheet.Cells[row, 7].Value = detail.SuggestedQuantity;
                sheet.Cells[row, 8].Value = detail.EstimatedSales;
                sheet.Cells[row, 9].Value = detail.DemandMet ? "✓" : "✗";
                sheet.Cells[row, 10].Value = detail.StockTargetMet ? "✓" : "✗";

                if (!detail.DemandMet)
                    sheet.Cells[row, 9].Style.Font.Color.SetColor(System.Drawing.Color.Red);
                if (!detail.StockTargetMet)
                    sheet.Cells[row, 10].Style.Font.Color.SetColor(System.Drawing.Color.Red);

                row++;
            }

            sheet.Cells.AutoFitColumns();
        }

        /// <summary>
        /// 写入原始数据
        /// </summary>
        private void WriteRawDataSheet(ExcelWorksheetWorksheet sheet, List<Product> products, List<MonthlyData> monthlyData)
        {
            sheet.Cells[1, 1].Value = "产品数据";
            sheet.Cells[1, 1].Style.Font.Bold = true;

            sheet.Cells[2, 1].Value = "产品编码";
            sheet.Cells[2, 2].Value = "产品名称";
            sheet.Cells[2, 3].Value = "单价(元)";
            sheet.Cells[2, 4].Value = "描述";

            int row = 3;
            foreach (Product product in products)
            {
                sheet.Cells[row, 1].Value = product.Code;
                sheet.Cells[row, 2].Value = product.Name;
                sheet.Cells[row, 3].Value = product.UnitPrice;
                sheet.Cells[row, 4].Value = product.Description;
                row++;
            }

            row += 2;
            sheet.Cells[row, 1].Value = "月度数据";
            sheet.Cells[row, 1].Style.Font.Bold = true;
            row++;

            sheet.Cells[row, 1].Value = "产品编码";
            sheet.Cells[row, 2].Value = "年份";
            sheet.Cells[row, 3].Value = "月份";
            sheet.Cells[row, 4].Value = "月度需求";
            sheet.Cells[row, 5].Value = "月末目标库存";
            sheet.Cells[row, 6].Value = "月初库存";
            row++;

            var productMap = products.ToDictionary(p => p.Id);
            foreach (MonthlyData data in monthlyData)
            {
                Product product = productMap.ContainsKey(data.ProductId) ? productMap[data.ProductId] : null;
                sheet.Cells[row, 1].Value = product != null ? product.Code : data.ProductId.ToString();
                sheet.Cells[row, 2].Value = data.Year;
                sheet.Cells[row, 3].Value = data.Month;
                sheet.Cells[row, 4].Value = data.MonthlyDemand;
                sheet.Cells[row, 5].Value = data.MonthEndTargetStock;
                sheet.Cells[row, 6].Value = data.MonthStartStock;
                row++;
            }

            sheet.Cells.AutoFitColumns();
        }
    }
}
