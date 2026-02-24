using System;
using System.Windows.Forms;
using ProductInventoryOptimizer.Models;

namespace ProductInventoryOptimizer.UI
{
    /// <summary>
    /// 产品编辑窗体
    /// </summary>
    public partial class ProductForm : Form
    {
        private readonly Product? _product;

        // UI控件
        private TextBox? _codeTextBox;
        private TextBox? _nameTextBox;
        private TextBox? _priceTextBox;
        private TextBox? _descTextBox;

        public ProductForm() : this(null) { }

        public ProductForm(Product? product)
        {
            _product = product;
            InitializeComponent();

            if (product != null)
            {
                _codeTextBox!.Text = product.Code;
                _nameTextBox!.Text = product.Name;
                _priceTextBox!.Text = product.UnitPrice.ToString("F2");
                _descTextBox!.Text = product.Description ?? "";
                this.Text = "编辑产品";
            }
            else
            {
                this.Text = "添加产品";
            }
        }

        /// <summary>
        /// 初始化窗体
        /// </summary>
        private void InitializeComponent()
        {
            this.Text = "产品信息";
            this.Size = new System.Drawing.Size(450, 350);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var padding = 15;
            var rowHeight = 35;
            var y = 20;

            // 产品编码
            var codeLabel = new Label
            {
                Text = "产品编码*:",
                Location = new System.Drawing.Point(padding, y),
                Size = new System.Drawing.Size(100, 23),
                TextAlign = System.Drawing.ContentAlignment.MiddleRight
            };
            _codeTextBox = new TextBox
            {
                Location = new System.Drawing.Point(120, y - 2),
                Size = new System.Drawing.Size(280, 25)
            };
            y += rowHeight;

            // 产品名称
            var nameLabel = new Label
            {
                Text = "产品名称*:",
                Location = new System.Drawing.Point(padding, y),
                Size = new System.Drawing.Size(100, 23),
                TextAlign = System.Drawing.ContentAlignment.MiddleRight
            };
            _nameTextBox = new TextBox
            {
                Location = new System.Drawing.Point(120, y - 2),
                Size = new System.Drawing.Size(280, 25)
            };
            y += rowHeight;

            // 单价
            var priceLabel = new Label
            {
                Text = "单价(元)*:",
                Location = new System.Drawing.Point(padding, y),
                Size = new System.Drawing.Size(100, 23),
                TextAlign = System.Drawing.ContentAlignment.MiddleRight
            };
            _priceTextBox = new TextBox
            {
                Location = new System.Drawing.Point(120, y - 2),
                Size = new System.Drawing.Size(150, 25)
            };
            y += rowHeight;

            // 描述
            var descLabel = new Label
            {
                Text = "描述:",
                Location = new System.Drawing.Point(padding, y),
                Size = new System.Drawing.Size(100, 23),
                TextAlign = System.Drawing.ContentAlignment.MiddleRight
            };
            _descTextBox = new TextBox
            {
                Location = new System.Drawing.Point(120, y - 2),
                Size = new System.Drawing.Size(280, 80),
                Multiline = true
            };
            y += 90;

            // 按钮
            var okButton = new Button
            {
                Text = "确定",
                DialogResult = DialogResult.OK,
                Location = new System.Drawing.Point(220, y),
                Size = new System.Drawing.Size(80, 30),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };
            okButton.Click += OkButton_Click;

            var cancelButton = new Button
            {
                Text = "取消",
                DialogResult = DialogResult.Cancel,
                Location = new System.Drawing.Point(320, y),
                Size = new System.Drawing.Size(80, 30),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };

            this.Controls.AddRange(new Control[]
            {
                codeLabel, _codeTextBox,
                nameLabel, _nameTextBox,
                priceLabel, _priceTextBox,
                descLabel, _descTextBox,
                okButton, cancelButton
            });

            this.AcceptButton = okButton;
            this.CancelButton = cancelButton;
        }

        /// <summary>
        /// 确定按钮点击事件
        /// </summary>
        private void OkButton_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_codeTextBox!.Text))
            {
                MessageBox.Show("请输入产品编码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _codeTextBox.Focus();
                this.DialogResult = DialogResult.None;
                return;
            }

            if (string.IsNullOrWhiteSpace(_nameTextBox!.Text))
            {
                MessageBox.Show("请输入产品名称", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _nameTextBox.Focus();
                this.DialogResult = DialogResult.None;
                return;
            }

            if (!decimal.TryParse(_priceTextBox!.Text, out var price) || price <= 0)
            {
                MessageBox.Show("请输入有效的单价", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _priceTextBox.Focus();
                this.DialogResult = DialogResult.None;
                return;
            }
        }

        /// <summary>
        /// 获取产品数据
        /// </summary>
        public Product GetProduct()
        {
            return new Product
            {
                Id = _product?.Id ?? 0,
                Code = _codeTextBox!.Text.Trim(),
                Name = _nameTextBox!.Text.Trim(),
                UnitPrice = decimal.Parse(_priceTextBox!.Text),
                Description = string.IsNullOrWhiteSpace(_descTextBox!.Text) ? null : _descTextBox.Text.Trim(),
                CreatedAt = _product?.CreatedAt ?? DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }
    }
}
