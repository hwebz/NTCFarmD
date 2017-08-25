﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gro.Infrastructure.Data.PrisgrafService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="PrisgrafService.IPrisgrafService")]
    public interface IPrisgrafService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrisgrafService/Ping", ReplyAction="http://tempuri.org/IPrisgrafService/PingResponse")]
        bool Ping();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrisgrafService/Ping", ReplyAction="http://tempuri.org/IPrisgrafService/PingResponse")]
        System.Threading.Tasks.Task<bool> PingAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrisgrafService/GetStartingPeriod", ReplyAction="http://tempuri.org/IPrisgrafService/GetStartingPeriodResponse")]
        int GetStartingPeriod(string ticket);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrisgrafService/GetStartingPeriod", ReplyAction="http://tempuri.org/IPrisgrafService/GetStartingPeriodResponse")]
        System.Threading.Tasks.Task<int> GetStartingPeriodAsync(string ticket);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrisgrafService/GetSelectablePeriods", ReplyAction="http://tempuri.org/IPrisgrafService/GetSelectablePeriodsResponse")]
        System.Collections.Generic.Dictionary<string, int> GetSelectablePeriods(string ticket);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrisgrafService/GetSelectablePeriods", ReplyAction="http://tempuri.org/IPrisgrafService/GetSelectablePeriodsResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<string, int>> GetSelectablePeriodsAsync(string ticket);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrisgrafService/GetChartLegendData", ReplyAction="http://tempuri.org/IPrisgrafService/GetChartLegendDataResponse")]
        System.Collections.Generic.Dictionary<int, string> GetChartLegendData(string ticket);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrisgrafService/GetChartLegendData", ReplyAction="http://tempuri.org/IPrisgrafService/GetChartLegendDataResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<int, string>> GetChartLegendDataAsync(string ticket);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrisgrafService/GetChartSeriesData", ReplyAction="http://tempuri.org/IPrisgrafService/GetChartSeriesDataResponse")]
        System.Collections.Generic.Dictionary<System.DateTime, double> GetChartSeriesData(System.DateTime fromDate, System.DateTime toDate, int articleNumber, string ticket);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrisgrafService/GetChartSeriesData", ReplyAction="http://tempuri.org/IPrisgrafService/GetChartSeriesDataResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<System.DateTime, double>> GetChartSeriesDataAsync(System.DateTime fromDate, System.DateTime toDate, int articleNumber, string ticket);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPrisgrafServiceChannel : Gro.Infrastructure.Data.PrisgrafService.IPrisgrafService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PrisgrafServiceClient : System.ServiceModel.ClientBase<Gro.Infrastructure.Data.PrisgrafService.IPrisgrafService>, Gro.Infrastructure.Data.PrisgrafService.IPrisgrafService {
        
        public PrisgrafServiceClient() {
        }
        
        public PrisgrafServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public PrisgrafServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PrisgrafServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PrisgrafServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool Ping() {
            return base.Channel.Ping();
        }
        
        public System.Threading.Tasks.Task<bool> PingAsync() {
            return base.Channel.PingAsync();
        }
        
        public int GetStartingPeriod(string ticket) {
            return base.Channel.GetStartingPeriod(ticket);
        }
        
        public System.Threading.Tasks.Task<int> GetStartingPeriodAsync(string ticket) {
            return base.Channel.GetStartingPeriodAsync(ticket);
        }
        
        public System.Collections.Generic.Dictionary<string, int> GetSelectablePeriods(string ticket) {
            return base.Channel.GetSelectablePeriods(ticket);
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<string, int>> GetSelectablePeriodsAsync(string ticket) {
            return base.Channel.GetSelectablePeriodsAsync(ticket);
        }
        
        public System.Collections.Generic.Dictionary<int, string> GetChartLegendData(string ticket) {
            return base.Channel.GetChartLegendData(ticket);
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<int, string>> GetChartLegendDataAsync(string ticket) {
            return base.Channel.GetChartLegendDataAsync(ticket);
        }
        
        public System.Collections.Generic.Dictionary<System.DateTime, double> GetChartSeriesData(System.DateTime fromDate, System.DateTime toDate, int articleNumber, string ticket) {
            return base.Channel.GetChartSeriesData(fromDate, toDate, articleNumber, ticket);
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<System.DateTime, double>> GetChartSeriesDataAsync(System.DateTime fromDate, System.DateTime toDate, int articleNumber, string ticket) {
            return base.Channel.GetChartSeriesDataAsync(fromDate, toDate, articleNumber, ticket);
        }
    }
}
