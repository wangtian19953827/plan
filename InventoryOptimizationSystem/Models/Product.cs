using System;

namespace ProductInventoryOptimizer.Models
{
    /// <summary>
    /// 产品信息模型
    /// </summary>
    public class Product
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 产品编码
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 单价（元）
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 产品描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return $"{Code} - {Name} (¥{UnitPrice:F2})";
        }
    }
}
