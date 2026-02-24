using System;
using System.Windows.Forms;

namespace ProductInventoryOptimizer.UI
{
    /// <summary>
    /// 批量设置月度数据窗体
    /// </summary>
    public partial class BatchMonthlyDataForm : Form
    {
        // UI控件
        private NumericUpDown? _demandNumeric;
        private NumericUpDown? _startStockNumeric;
        private NumericUpDown? _endStockNumeric;

        public BatchMonthlyDataForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化窗体
        /// </summary>
        private void InitializeComponent()
        {
            this.Text = "批量设置月度数据";
            this.Size = new System.Drawing.Size(350, 250);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var padding = 15;
            var rowHeight = 50;
            var y = 20;

            // 月度需求
            var demandLabel = new Label
            {
                Text = "月度需求量:",
                Location = new System.Drawing.Point(padding, y),
                Size = new System.Drawing.Size(120, 23),
                TextAlign = System.Drawing.ContentAlignment.MiddleRight
            };
            _demandNumeric = new NumericUpDown
            {
                Location = new System.Drawing.Point(140, y - 3),
                Size = new System.Drawing.Size(120, 25),
                Minimum = 0,
                Maximum = 100000,
                Value = 100
            };
            y += rowHeight;

            // 月初库存
            var startStockLabel = new Label
            {
                Text = "月初库存:",
                Location = new System.Drawing.Point(padding, y),
                Size = new System.Drawing.Size(120, 23),
                TextAlign = System.Drawing.ContentAlignment.MiddleRight
            };
            _startStockNumeric = new NumericUpDown
            {
                Location = new System.Drawing.Point(140, y - 3),
                Size = new System.Drawing.Size(120, 25),
                Minimum = 0,
                Maximum = 100000,
                Value = 50
            };
            y += rowHeight;

            // 月末目标库存
            var endStockLabel = new Label
            {
                Text = "月末目标库存:",
                Location = new System.Drawing.Point(padding, y),
                Size = new System.Drawing.Size(120, 23),
                TextAlign = System.Drawing.ContentAlignment.MiddleRight
            };
            _endStockNumeric = new NumericUpDown
            {
                Location = new System.Drawing.Point(140, y - 3),
                Size = new System.Drawing.Size(120, 25),
                Minimum = 0,
                Maximum = 100000,
                Value = 50
            };
            y += rowHeight + 10;

            // 说明标签
            var noteLabel = new Label
            {
                Text = "注意：此操作将修改所有产品的月度数据",
                Location = new System.Drawing.Point(padding, y),
                Size = new System.Drawing.Size(300, 23),
                ForeColor = System.Drawing.Color.Red
            };
            y += 30;

            // 按钮
            var okButton = new Button
            {
                Text = "确定",
                DialogResult = DialogResult.OK,
                Location = new System.Drawing.Point(180, y),
                Size = new System.Drawing.Size(80, 30)
            };

            var cancelButton = new Button
            {
                Text = "取消",
                DialogResult = DialogResult.Cancel,
                Location = new System.Drawing.Point(270, y),
                Size = new System.Drawing.Size(80, 30)
            };

            this.Controls.AddRange(new Control[]
            {
                demandLabel, _demandNumeric,
                startStockLabel, _startStockNumeric,
                endStockLabel, _endStockNumeric,
                noteLabel,
                okButton, cancelButton
            });

            this.AcceptButton = okButton;
            this.CancelButton = cancelButton;
        }

        /// <summary>
        /// 获取批量设置的值
        /// </summary>
        public (int demand, int startStock, int endStock) GetValues()
        {
            return (
                (int)_demandNumeric!.Value,
                (int)_startStockNumeric!.Value,
                (int)_endStockNumeric!.Value
            );
        }
    }
}
