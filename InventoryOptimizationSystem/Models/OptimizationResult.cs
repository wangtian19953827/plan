using System;
using System.Collections.Generic;

namespace ProductInventoryOptimizer.Models
{
    /// <summary>
    /// 优化计算结果
    /// </summary>
    public class OptimizationResult
    {
        /// <summary>
        /// 是否成功找到解
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 求解状态描述
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 目标总销售额
        /// </summary>
        public decimal TargetSalesAmount { get; set; }

        /// <summary>
        /// 计算得出的总销售额
        /// </summary>
        public decimal CalculatedSalesAmount { get; set; }

        /// <summary>
        /// 销售额差额
        /// </summary>
        public decimal SalesGap => CalculatedSalesAmount - TargetSalesAmount;

        /// <summary>
        /// 目标总供应量上限
        /// </summary>
        public int MaxTotalSupply { get; set; }

        /// <summary>
        /// 实际总供应量
        /// </summary>
        public int ActualTotalSupply { get; set; }

        /// <summary>
        /// 是否满足供应量约束
        /// </summary>
        public bool SupplyConstraintMet => ActualTotalSupply <= MaxTotalSupply;

        /// <summary>
        /// 每个产品的优化结果
        /// </summary>
        public List<ProductOptimizationDetail> ProductDetails { get; set; } = new();

        /// <summary>
        /// 计算时间（毫秒）
        /// </summary>
        public long ComputationTimeMs { get; set; }

        /// <summary>
        /// 计算完成时间
        /// </summary>
        public DateTime ComputedAt { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// 单个产品的优化详情
    /// </summary>
    public class ProductOptimizationDetail
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; } = string.Empty;

        /// <summary>
        /// 单价
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 月度需求量
        /// </summary>
        public int MonthlyDemand { get; set; }

        /// <summary>
        /// 月初库存
        /// </summary>
        public int MonthStartStock { get; set; }

        /// <summary>
        /// 月末目标库存
        /// </summary>
        public int MonthEndTargetStock { get; set; }

        /// <summary>
        /// 建议生产/销售数量
        /// </summary>
        public int SuggestedQuantity { get; set; }

        /// <summary>
        /// 预计销售额
        /// </summary>
        public decimal EstimatedSales => SuggestedQuantity * UnitPrice;

        /// <summary>
        /// 月末实际库存（月初库存 + 生产 - 销售）
        /// </summary>
        public int ActualMonthEndStock => MonthStartStock + SuggestedQuantity - SuggestedQuantity; // 假设生产=销售

        /// <summary>
        /// 是否满足月末库存目标
        /// </summary>
        public bool StockTargetMet => ActualMonthEndStock >= MonthEndTargetStock;

        /// <summary>
        /// 是否满足需求
        /// </summary>
        public bool DemandMet => SuggestedQuantity >= MonthlyDemand;
    }
}
