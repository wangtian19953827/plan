namespace ProductInventoryOptimizer.Models
{
    /// <summary>
    /// 月度数据模型
    /// </summary>
    public class MonthlyData
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 月份 (1-12)
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// 月度需求量
        /// </summary>
        public int MonthlyDemand { get; set; }

        /// <summary>
        /// 月末目标库存
        /// </summary>
        public int MonthEndTargetStock { get; set; }

        /// <summary>
        /// 月初实际库存
        /// </summary>
        public int MonthStartStock { get; set; }
    }
}
