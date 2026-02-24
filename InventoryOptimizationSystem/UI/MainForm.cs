using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ProductInventoryOptimizer.Models;
using ProductInventoryOptimizer.Services;

namespace ProductInventoryOptimizer.UI
{
    /// <summary>
    /// 主窗体
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly OptimizationService _optimizationService;
        private readonly ExcelService _excelService;

        // 数据存储
        private List<Product> _products = new();
        private List<MonthlyData> _monthlyData = new();
        private OptimizationResult? _lastResult;

        // UI控件
        private TabControl? _tabControl;
        private DataGridView? _productsGrid;
        private DataGridView? _monthlyDataGrid;
        private TextBox? _maxSupplyTextBox;
        private TextBox? _targetSalesTextBox;
        private Button? _calculateButton;
        private Button? _importButton;
        private Button? _exportButton;
        private Label? _statusLabel;
        private Panel? _resultPanel;
        private DataGridView? _resultGrid;
        private Label? _summaryLabel;

        public MainForm()
        {
            _optimizationService = new OptimizationService();
            _excelService = new ExcelService();

            InitializeComponent();
            InitializeDataGrids();
            LoadSampleData();
        }

        /// <summary>
        /// 初始化窗体
        /// </summary>
        private void InitializeComponent()
        {
            this.Text = "产品销量优化与库存管理系统";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1000, 600);

            // 创建主菜单
            var menuStrip = new MenuStrip();
            var fileMenu = new ToolStripMenuItem("文件(&F)");
            var importItem = new ToolStripMenuItem("导入Excel(&I)", null, (s, e) => ImportExcel());
            var exportItem = new ToolStripMenuItem("导出结果(&E)", null, (s, e) => ExportResult());
            var exitItem = new ToolStripMenuItem("退出(&X)", null, (s, e) => this.Close());

            fileMenu.DropDownItems.AddRange(new[] { importItem, exportItem, new ToolStripSeparator(), exitItem });
            menuStrip.Items.Add(fileMenu);
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);

            // 创建工具栏
            var toolStrip = new ToolStrip();
            toolStrip.Dock = DockStyle.Top;
            toolStrip.GripStyle = ToolStripGripStyle.Hidden;

            _importButton = new ToolStripButton("导入Excel", null, (s, e) => ImportExcel())
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };
            _exportButton = new ToolStripButton("导出结果", null, (s, e) => ExportResult())
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                Enabled = false
            };
            _calculateButton = new ToolStripButton("开始计算", null, (s, e) => CalculateOptimization())
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                ForeColor = Color.Green,
                Font = new Font("Microsoft YaHei", 9, FontStyle.Bold)
            };

            toolStrip.Items.AddRange(new ToolStripItem[] { _importButton, new ToolStripSeparator(), _calculateButton, _exportButton });
            this.Controls.Add(toolStrip);

            // 创建参数面板
            var paramPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(240, 248, 255),
                Padding = new Padding(10)
            };

            var maxSupplyLabel = new Label
            {
                Text = "本月可供应总数:",
                Location = new Point(10, 20),
                AutoSize = true
            };
            _maxSupplyTextBox = new TextBox
            {
                Location = new Point(130, 17),
                Size = new Size(100, 25),
                Text = "10000"
            };

            var targetSalesLabel = new Label
            {
                Text = "目标销售额(元):",
                Location = new Point(250, 20),
                AutoSize = true
            };
            _targetSalesTextBox = new TextBox
            {
                Location = new Point(360, 17),
                Size = new Size(120, 25),
                Text = "500000"
            };

            paramPanel.Controls.AddRange(new Control[] { maxSupplyLabel, _maxSupplyTextBox, targetSalesLabel, _targetSalesTextBox });
            this.Controls.Add(paramPanel);

            // 创建选项卡控件
            _tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Padding = new Point(5, 5)
            };

            // 产品管理选项卡
            var productTabPage = new TabPage("产品管理");
            var productPanel = new Panel { Dock = DockStyle.Fill };

            var productToolbar = new ToolStrip
            {
                Dock = DockStyle.Top,
                GripStyle = ToolStripGripStyle.Hidden
            };
            productToolbar.Items.Add(new ToolStripButton("添加产品", null, (s, e) => AddProduct())
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text
            });
            productToolbar.Items.Add(new ToolStripButton("编辑产品", null, (s, e) => EditProduct())
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text
            });
            productToolbar.Items.Add(new ToolStripButton("删除产品", null, (s, e) => DeleteProduct())
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text
            });

            _productsGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AlternatingRowsDefaultCellStyle = { BackColor = Color.FromArgb(245, 245, 245) }
            };

            productPanel.Controls.AddRange(new Control[] { productToolbar, _productsGrid });
            productTabPage.Controls.Add(productPanel);

            // 月度数据选项卡
            var monthlyTabPage = new TabPage("月度数据");
            var monthlyPanel = new Panel { Dock = DockStyle.Fill };

            var monthlyToolbar = new ToolStrip
            {
                Dock = DockStyle.Top,
                GripStyle = ToolStripGripStyle.Hidden
            };
            monthlyToolbar.Items.Add(new ToolStripButton("编辑数据", null, (s, e) => EditMonthlyData())
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text
            });
            monthlyToolbar.Items.Add(new ToolStripButton("批量设置", null, (s, e) => BatchSetMonthlyData())
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text
            });

            _monthlyDataGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AlternatingRowsDefaultCellStyle = { BackColor = Color.FromArgb(245, 245, 245) }
            };

            monthlyPanel.Controls.AddRange(new Control[] { monthlyToolbar, _monthlyDataGrid });
            monthlyTabPage.Controls.Add(monthlyPanel);

            // 计算结果选项卡
            var resultTabPage = new TabPage("计算结果");
            var resultPanel = new Panel { Dock = DockStyle.Fill };

            _resultPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120,
                BackColor = Color.FromArgb(240, 255, 240),
                Padding = new Padding(10),
                Visible = false
            };

            _summaryLabel = new Label
            {
                Location = new Point(10, 10),
                Size = new Size(1100, 100),
                Font = new Font("Microsoft YaHei", 10),
                BackColor = Color.Transparent
            };

            _resultPanel.Controls.Add(_summaryLabel);

            _resultGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AlternatingRowsDefaultCellStyle = { BackColor = Color.FromArgb(245, 245, 245) }
            };

            resultPanel.Controls.AddRange(new Control[] { _resultPanel, _resultGrid });
            resultTabPage.Controls.Add(resultPanel);

            _tabControl.TabPages.AddRange(new[] { productTabPage, monthlyTabPage, resultTabPage });
            this.Controls.Add(_tabControl);

            // 状态栏
            var statusStrip = new StatusStrip();
            _statusLabel = new ToolStripStatusLabel("就绪");
            statusStrip.Items.Add(_statusLabel);
            this.Controls.Add(statusStrip);
        }

        /// <summary>
        /// 初始化数据网格列
        /// </summary>
        private void InitializeDataGrids()
        {
            // 产品网格列
            _productsGrid!.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "产品编码",
                DataPropertyName = "Code",
                Width = 120
            });
            _productsGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "产品名称",
                DataPropertyName = "Name",
                Width = 200
            });
            _productsGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "单价(元)",
                DataPropertyName = "UnitPrice",
                Width = 100,
                DefaultCellStyle = { Format = "F2" }
            });
            _productsGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "描述",
                DataPropertyName = "Description",
                Width = 300
            });

            // 月度数据网格列
            _monthlyDataGrid!.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "产品编码",
                DataPropertyName = "ProductCode",
                Width = 120
            });
            _monthlyDataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "产品名称",
                DataPropertyName = "ProductName",
                Width = 180
            });
            _monthlyDataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "年份",
                DataPropertyName = "Year",
                Width = 80
            });
            _monthlyDataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "月份",
                DataPropertyName = "Month",
                Width = 60
            });
            _monthlyDataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "月度需求",
                DataPropertyName = "MonthlyDemand",
                Width = 100
            });
            _monthlyDataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "月初库存",
                DataPropertyName = "MonthStartStock",
                Width = 100
            });
            _monthlyDataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "月末目标库存",
                DataPropertyName = "MonthEndTargetStock",
                Width = 120
            });

            // 结果网格列
            _resultGrid!.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "产品编码",
                DataPropertyName = "ProductCode",
                Width = 100
            });
            _resultGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "产品名称",
                DataPropertyName = "ProductName",
                Width = 150
            });
            _resultGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "单价",
                DataPropertyName = "UnitPrice",
                Width = 80,
                DefaultCellStyle = { Format = "F2" }
            });
            _resultGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "需求",
                DataPropertyName = "MonthlyDemand",
                Width = 70
            });
            _resultGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "建议销量",
                DataPropertyName = "SuggestedQuantity",
                Width = 90,
                DefaultCellStyle = { BackColor = Color.LightYellow, Font = new Font("Microsoft YaHei", 9, FontStyle.Bold) }
            });
            _resultGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "预计销售额",
                DataPropertyName = "EstimatedSales",
                Width = 100,
                DefaultCellStyle = { Format = "C2" }
            });
            _resultGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "需求满足",
                DataPropertyName = "DemandMet",
                Width = 80
            });
            _resultGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "库存满足",
                DataPropertyName = "StockTargetMet",
                Width = 80
            });
        }

        /// <summary>
        /// 加载示例数据
        /// </summary>
        private void LoadSampleData()
        {
            // 创建示例产品（约80个）
            for (int i = 1; i <= 80; i++)
            {
                _products.Add(new Product
                {
                    Id = i,
                    Code = $"P{i:D3}",
                    Name = $"产品{i}",
                    UnitPrice = 100 + i * 10, // 单价在100-900之间
                    Description = $"示例产品{i}的描述信息"
                });
            }

            // 创建示例月度数据
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;

            foreach (var product in _products)
            {
                _monthlyData.Add(new MonthlyData
                {
                    ProductId = product.Id,
                    Year = currentYear,
                    Month = currentMonth,
                    MonthlyDemand = 50 + (product.Id % 50) * 10, // 需求在50-550之间
                    MonthStartStock = 100 + (product.Id % 30) * 5, // 月初库存
                    MonthEndTargetStock = 80 + (product.Id % 40) * 5 // 月末目标库存
                });
            }

            RefreshDataGrids();
            UpdateStatus($"已加载 {_products.Count} 个产品和 {_monthlyData.Count} 条月度数据");
        }

        /// <summary>
        /// 刷新数据网格
        /// </summary>
        private void RefreshDataGrids()
        {
            _productsGrid!.DataSource = new BindingSource
            {
                DataSource = _products
            };

            var monthlyDisplayList = _monthlyData.Select(m =>
            {
                var product = _products.FirstOrDefault(p => p.Id == m.ProductId);
                return new
                {
                    m.ProductId,
                    ProductCode = product?.Code ?? m.ProductId.ToString(),
                    ProductName = product?.Name ?? "",
                    m.Year,
                    m.Month,
                    m.MonthlyDemand,
                    m.MonthStartStock,
                    m.MonthEndTargetStock
                };
            }).ToList();

            _monthlyDataGrid!.DataSource = new BindingSource
            {
                DataSource = monthlyDisplayList
            };
        }

        /// <summary>
        /// 导入Excel
        /// </summary>
        private void ImportExcel()
        {
            using var dialog = new OpenFileDialog
            {
                Filter = "Excel文件|*.xlsx;*.xls",
                Title = "选择要导入的Excel文件"
            };

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            UpdateStatus("正在导入Excel...");
            Cursor = Cursors.WaitCursor;

            try
            {
                var (products, monthlyData, message) = _excelService.ImportFromExcel(dialog.FileName);

                if (products.Count > 0)
                {
                    _products = products;
                    _monthlyData = monthlyData;
                    RefreshDataGrids();
                    UpdateStatus($"导入成功: {message}");
                }
                else
                {
                    MessageBox.Show($"导入失败: {message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导入异常: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 导出结果
        /// </summary>
        private void ExportResult()
        {
            if (_lastResult == null)
            {
                MessageBox.Show("请先进行优化计算", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var dialog = new SaveFileDialog
            {
                Filter = "Excel文件|*.xlsx",
                Title = "导出计算结果",
                FileName = $"优化结果_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
            };

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            UpdateStatus("正在导出结果...");
            Cursor = Cursors.WaitCursor;

            try
            {
                var (success, message) = _excelService.ExportResultToExcel(
                    _lastResult,
                    _products,
                    _monthlyData,
                    dialog.FileName);

                if (success)
                {
                    MessageBox.Show(message, "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateStatus(message);
                }
                else
                {
                    MessageBox.Show(message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出异常: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 执行优化计算
        /// </summary>
        private void CalculateOptimization()
        {
            if (!int.TryParse(_maxSupplyTextBox!.Text, out var maxSupply) || maxSupply <= 0)
            {
                MessageBox.Show("请输入有效的供应总数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(_targetSalesTextBox!.Text, out var targetSales) || targetSales <= 0)
            {
                MessageBox.Show("请输入有效的目标销售额", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_products.Count == 0)
            {
                MessageBox.Show("请先添加产品", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            UpdateStatus("正在计算优化方案...");
            Cursor = Cursors.WaitCursor;
            _tabControl!.SelectedTab = _tabControl.TabPages[2]; // 切换到结果选项卡

            try
            {
                _lastResult = _optimizationService.Optimize(
                    _products,
                    _monthlyData,
                    maxSupply,
                    targetSales);

                DisplayResult(_lastResult);

                if (_lastResult.Success)
                {
                    _exportButton!.Enabled = true;
                    UpdateStatus($"计算完成: {_lastResult.Status} (耗时 {_lastResult.ComputationTimeMs}ms)");
                }
                else
                {
                    MessageBox.Show($"计算失败: {_lastResult.Status}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"计算异常: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 显示计算结果
        /// </summary>
        private void DisplayResult(OptimizationResult result)
        {
            _resultPanel!.Visible = true;

            var summaryText = $"【计算结果】\n" +
                $"求解状态: {result.Status}\n" +
                $"计算耗时: {result.ComputationTimeMs} 毫秒\n\n" +
                $"目标销售额: {result.TargetSalesAmount:C2}\n" +
                $"实际销售额: {result.CalculatedSalesAmount:C2}\n" +
                $"销售额差额: {result.SalesGap:C2}\n\n" +
                $"最大供应量: {result.MaxTotalSupply}\n" +
                $"实际供应量: {result.ActualTotalSupply}\n" +
                $"使用率: {(result.ActualTotalSupply * 100.0 / result.MaxTotalSupply):F2}%";

            _summaryLabel!.Text = summaryText;
            _summaryLabel.BackColor = result.Success ? Color.FromArgb(240, 255, 240) : Color.FromArgb(255, 240, 240);

            _resultGrid!.DataSource = new BindingSource
            {
                DataSource = result.ProductDetails
            };
        }

        /// <summary>
        /// 添加产品
        /// </summary>
        private void AddProduct()
        {
            var form = new ProductForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                var product = form.GetProduct();
                product.Id = _products.Count > 0 ? _products.Max(p => p.Id) + 1 : 1;
                _products.Add(product);

                // 同时创建月度数据
                _monthlyData.Add(new MonthlyData
                {
                    ProductId = product.Id,
                    Year = DateTime.Now.Year,
                    Month = DateTime.Now.Month,
                    MonthlyDemand = 0,
                    MonthStartStock = 0,
                    MonthEndTargetStock = 0
                });

                RefreshDataGrids();
                UpdateStatus($"已添加产品: {product.Name}");
            }
        }

        /// <summary>
        /// 编辑产品
        /// </summary>
        private void EditProduct()
        {
            if (_productsGrid!.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要编辑的产品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var product = (Product)_productsGrid.SelectedRows[0].DataBoundItem;
            var form = new ProductForm(product);

            if (form.ShowDialog() == DialogResult.OK)
            {
                var updatedProduct = form.GetProduct();
                product.Name = updatedProduct.Name;
                product.Code = updatedProduct.Code;
                product.UnitPrice = updatedProduct.UnitPrice;
                product.Description = updatedProduct.Description;
                product.UpdatedAt = DateTime.Now;

                RefreshDataGrids();
                UpdateStatus($"已更新产品: {product.Name}");
            }
        }

        /// <summary>
        /// 删除产品
        /// </summary>
        private void DeleteProduct()
        {
            if (_productsGrid!.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要删除的产品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var product = (Product)_productsGrid.SelectedRows[0].DataBoundItem;
            var result = MessageBox.Show(
                $"确定要删除产品 \"{product.Name}\" 吗？",
                "确认删除",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                _products.Remove(product);
                _monthlyData.RemoveAll(m => m.ProductId == product.Id);
                RefreshDataGrids();
                UpdateStatus($"已删除产品: {product.Name}");
            }
        }

        /// <summary>
        /// 编辑月度数据
        /// </summary>
        private void EditMonthlyData()
        {
            if (_monthlyDataGrid!.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要编辑的数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var rowIndex = _monthlyDataGrid.SelectedRows[0].Index;
            var data = _monthlyData[rowIndex];
            var product = _products.FirstOrDefault(p => p.Id == data.ProductId);

            var form = new MonthlyDataForm(product, data);
            if (form.ShowDialog() == DialogResult.OK)
            {
                var updatedData = form.GetData();
                data.MonthlyDemand = updatedData.MonthlyDemand;
                data.MonthStartStock = updatedData.MonthStartStock;
                data.MonthEndTargetStock = updatedData.MonthEndTargetStock;
                data.Year = updatedData.Year;
                data.Month = updatedData.Month;

                RefreshDataGrids();
                UpdateStatus($"已更新月度数据: {product?.Name}");
            }
        }

        /// <summary>
        /// 批量设置月度数据
        /// </summary>
        private void BatchSetMonthlyData()
        {
            var form = new BatchMonthlyDataForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                var (demand, startStock, endStock) = form.GetValues();

                foreach (var data in _monthlyData)
                {
                    data.MonthlyDemand = demand;
                    data.MonthStartStock = startStock;
                    data.MonthEndTargetStock = endStock;
                }

                RefreshDataGrids();
                UpdateStatus($"已批量设置月度数据");
            }
        }

        /// <summary>
        /// 更新状态栏
        /// </summary>
        private void UpdateStatus(string message)
        {
            _statusLabel!.Text = $"[{DateTime.Now:HH:mm:ss}] {message}";
        }
    }
}
