using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Google.OrTools.LinearSolver;
using ProductInventoryOptimizer.Models;

namespace ProductInventoryOptimizer.Services
{
    /// <summary>
    /// 约束优化服务 - 使用 Google OR-Tools 进行线性规划求解
    /// </summary>
    public class OptimizationService
    {
        /// <summary>
        /// 执行优化计算
        /// </summary>
        public OptimizationResult Optimize(
            List<Product> products,
            List<MonthlyData> monthlyDataList,
            int maxTotalSupply,
            decimal targetSalesAmount)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = new OptimizationResult
            {
                TargetSalesAmount = targetSalesAmount,
                MaxTotalSupply = maxTotalSupply
            };

            try
            {
                // 创建求解器（使用 CBC 求解器）
                Solver solver = Solver.CreateSolver("CBC");
                if (solver == null)
                {
                    result.Status = "无法创建求解器";
                    return result;
                }

                // 创建决策变量：每个产品的生产/销售数量
                var variables = new Dictionary<int, Variable>();
                var productDataMap = monthlyDataList.ToDictionary(m => m.ProductId);

                foreach (var product in products)
                {
                    // 决策变量：非负整数
                    string varName = "x_" + product.Id;
                    Variable variable = solver.MakeIntVar(0, maxTotalSupply, varName);
                    variables[product.Id] = variable;
                }

                // ========== 约束条件 ==========

                // 约束1：总供应量上限（硬约束）
                Constraint supplyConstraint = solver.MakeConstraint(0, maxTotalSupply, "总供应量约束");
                foreach (var product in products)
                {
                    supplyConstraint.SetCoefficient(variables[product.Id], 1);
                }

                // ========== 目标函数 ==========

                // 目标：最大化销售额，同时尽可能满足需求
                Objective objective = solver.Objective();
                objective.SetMaximization();

                // 销售额权重
                const double salesWeight = 1.0;
                // 需求满足惩罚权重（未满足需求会减少目标值）
                const double demandPenaltyWeight = 1000.0;

                foreach (var product in products)
                {
                    Variable variable = variables[product.Id];

                    // 1. 最大化销售额
                    objective.SetCoefficient(variable, (double)product.UnitPrice * salesWeight);

                    // 2. 惩罚未满足的需求
                    if (productDataMap.TryGetValue(product.Id, out MonthlyData data) && data.MonthlyDemand > 0)
                    {
                        // 创建辅助变量表示需求未满足量
                        Variable demandShortfall = solver.MakeNumVar(0, double.MaxValue, "shortfall_" + product.Id);

                        // 约束：生产量 + 需求未满足量 >= 需求量
                        Constraint demandConstraint = solver.MakeConstraint(data.MonthlyDemand, double.MaxValue, "需求约束_" + product.Id);
                        demandConstraint.SetCoefficient(variable, 1);
                        demandConstraint.SetCoefficient(demandShortfall, 1);

                        // 在目标函数中惩罚需求未满足
                        objective.SetCoefficient(demandShortfall, -demandPenaltyWeight);
                    }
                }

                // ========== 求解 ==========

                Solver.ResultStatus solverStatus = solver.Solve();

                // ========== 处理结果 ==========

                switch (solverStatus)
                {
                    case Solver.ResultStatus.OPTIMAL:
                        result.Success = true;
                        result.Status = "找到最优解";
                        break;

                    case Solver.ResultStatus.FEASIBLE:
                        result.Success = true;
                        result.Status = "找到可行解（非最优）";
                        break;

                    default:
                        result.Success = false;
                        result.Status = GetStatusString(solverStatus);
                        return result;
                }

                // 提取结果
                foreach (var product in products)
                {
                    Variable variable = variables[product.Id];
                    int suggestedQty = (int)Math.Round(variable.SolutionValue());

                    MonthlyData productData = productDataMap.ContainsKey(product.Id)
                        ? productDataMap[product.Id]
                        : new MonthlyData { ProductId = product.Id };

                    ProductOptimizationDetail detail = new ProductOptimizationDetail
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        ProductCode = product.Code,
                        UnitPrice = product.UnitPrice,
                        MonthlyDemand = productData.MonthlyDemand,
                        MonthStartStock = productData.MonthStartStock,
                        MonthEndTargetStock = productData.MonthEndTargetStock,
                        SuggestedQuantity = suggestedQty
                    };

                    result.ProductDetails.Add(detail);
                }

                // 计算汇总数据
                result.ActualTotalSupply = result.ProductDetails.Sum(d => d.SuggestedQuantity);
                result.CalculatedSalesAmount = result.ProductDetails.Sum(d => d.EstimatedSales);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Status = "求解异常: " + ex.Message;
            }

            stopwatch.Stop();
            result.ComputationTimeMs = stopwatch.ElapsedMilliseconds;

            return result;
        }

        /// <summary>
        /// 获取求解器状态字符串
        /// </summary>
        private static string GetStatusString(Solver.ResultStatus status)
        {
            switch (status)
            {
                case Solver.ResultStatus.OPTIMAL:
                    return "最优解";
                case Solver.ResultStatus.FEASIBLE:
                    return "可行解";
                case Solver.ResultStatus.INFEASIBLE:
                    return "无解（约束冲突）";
                case Solver.ResultStatus.UNBOUNDED:
                    return "无界";
                case Solver.ResultStatus.ABNORMAL:
                    return "异常";
                case Solver.ResultStatus.MODEL_INVALID:
                    return "模型无效";
                case Solver.ResultStatus.NOT_SOLVED:
                    return "未求解";
                default:
                    return "未知状态";
            }
        }
    }
}
