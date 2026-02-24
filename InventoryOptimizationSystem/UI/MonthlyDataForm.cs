using System;
using System.Windows.Forms;
using ProductInventoryOptimizer.Models;

namespace ProductInventoryOptimizer.UI
{
    /// <summary>
    /// 月度数据编辑窗体
    /// </summary>
    public partial class MonthlyDataForm : Form
    {
        private readonly Product? _product;
        private readonly MonthlyData _data;

        // UI控件
        private NumericUpDown? _yearNumeric;
        private NumericUpDown? _monthNumeric;
        private NumericUpDown? _demandNumeric;
        private NumericUpDown? _startStockNumeric;
        private NumericUpDown? _endStockNumeric;

        public MonthlyDataForm(Product? product, MonthlyData data)
        {
            _product = product;
            _data = data;
            InitializeComponent();
            LoadData();
        }

        /// <summary>
        /// 初始化窗体
        /// </summary>
        private void InitializeComponent()
        {
            this.Text = _product != null ? $"月度数据 - {_product.Name}" : "月度数据";
            this.Size = new System.Drawing.Size(400, 350);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var padding = 15;
            var rowHeight = 50;
            var y = 20;

            // 年份
            var yearLabel = new Label
            {
                Text = "年份:",
                Location = new System.Drawing.Point(padding, y),
                Size = new System.Drawing.Size(100, 23),
                TextAlign = System.Drawing.ContentAlignment.MiddleRight
            };
            _yearNumeric = new NumericUpDown
            {
                Location = new System.Drawing.Point(120, y - 3),
                Size = new System.Drawing.Size(100, 25),
                Minimum = 2020,
                Maximum = 2030,
                Value = DateTime.Now.Year
            };
            y += rowHeight;

            // 月份
            var monthLabel = new Label
            {
                Text = "月份:",
                Location = new System.Drawing.Point(padding, y),
                Size = new System.Drawing.Size(100, 23),
                TextAlign = System.Drawing.ContentAlignment.MiddleRight
            };
            _monthNumeric = new NumericUpDown
            {
                Location = new System.Drawing.Point(120, y - 3),
                Size = new System.Drawing.Size(100, 25),
                Minimum = 1,
                Maximum = 12,
                Value = DateTime.Now.Month
            };
            y += rowHeight;

            // 月度需求
            var demandLabel = new Label
            {
                Text = "月度需求量:",
                Location = new System.Drawing.Point(padding, y),
                Size = new System.Drawing.Size(100, 23),
                TextAlign = System.Drawing.ContentAlignment.MiddleRight
            };
            _demandNumeric = new NumericUpDown
            {
                Location = new System.Drawing.Point(120, y - 3),
                Size = new System.Drawing.Size(120, 25),
                Minimum = 0,
                Maximum = 100000
            };
            y += rowHeight;

            // 月初库存
            var startStockLabel = new Label
            {
                Text = "月初库存:",
                Location = new System.Drawing.Point(padding, y),
                Size = new System.Drawing.Size(100, 23),
                TextAlign = System.Drawing.ContentAlignment.MiddleRight
            };
            _startStockNumeric = new NumericUpDown
            {
                Location = new System.Drawing.Point(120, y - 3),
                Size = new System.Drawing.Size(120, 25),
                Minimum = 0,
                Maximum = 100000
            };
            y += rowHeight;

            // 月末目标库存
            var endStockLabel = new Label
            {
                Text = "月末目标库存:",
                Location = new System.Drawing.Point(padding, y),
                Size = new System.Drawing.Size(100, 23),
                TextAlign = System.Drawing.ContentAlignment.MiddleRight
            };
            _endStockNumeric = new NumericUpDown
            {
                Location = new System.Drawing.Point(120, y - 3),
                Size = new System.Drawing.Size(120, 25),
                Minimum = 0,
                Maximum = 100000
            };
            y += rowHeight + 10;

            // 按钮
            var okButton = new Button
            {
                Text = "确定",
                DialogResult = DialogResult.OK,
                Location = new System.Drawing.Point(220, y),
                Size = new System.Drawing.Size(80, 30)
            };

            var cancelButton = new Button
            {
                Text = "取消",
                DialogResult = DialogResult.Cancel,
                Location = new System.Drawing.Point(320, y),
                Size = new System.Drawing.Size(80, 30)
            };

            this.Controls.AddRange(new Control[]
            {
                yearLabel, _yearNumeric,
                monthLabel, _monthNumeric,
                demandLabel, _demandNumeric,
                startStockLabel, _startStockNumeric,
                endStockLabel, _endStockNumeric,
                okButton, cancelButton
            });

            this.AcceptButton = okButton;
            this.CancelButton = cancelButton;
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            _yearNumeric!.Value = _data.Year;
            _monthNumeric!.Value = _data.Month;
            _demandNumeric!.Value = _data.MonthlyDemand;
            _startStockNumeric!.Value = _data.MonthStartStock;
            _endStockNumeric!.Value = _data.MonthEndTargetStock;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        public MonthlyData GetData()
        {
            return new MonthlyData
            {
                ProductId = _data.ProductId,
                Year = (int)_yearNumeric!.Value,
                Month = (int)_monthNumeric!.Value,
                MonthlyDemand = (int)_demandNumeric!.Value,
                MonthStartStock = (int)_startStockNumeric!.Value,
                MonthEndTargetStock = (int)_endStockNumeric!.Value
            };
        }
    }
}
