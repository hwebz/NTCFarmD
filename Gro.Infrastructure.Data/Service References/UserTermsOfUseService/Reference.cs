﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gro.Infrastructure.Data.UserTermsOfUseService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="UserTermsOfUseService.IUserTermsOfUseService")]
    public interface IUserTermsOfUseService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserTermsOfUseService/CheckTerms", ReplyAction="http://tempuri.org/IUserTermsOfUseService/CheckTermsResponse")]
        bool CheckTerms(string terms, int version, string ticket);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserTermsOfUseService/CheckTerms", ReplyAction="http://tempuri.org/IUserTermsOfUseService/CheckTermsResponse")]
        System.Threading.Tasks.Task<bool> CheckTermsAsync(string terms, int version, string ticket);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserTermsOfUseService/CheckUserAccepts", ReplyAction="http://tempuri.org/IUserTermsOfUseService/CheckUserAcceptsResponse")]
        bool CheckUserAccepts(int userId, string ticket, string terms);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserTermsOfUseService/CheckUserAccepts", ReplyAction="http://tempuri.org/IUserTermsOfUseService/CheckUserAcceptsResponse")]
        System.Threading.Tasks.Task<bool> CheckUserAcceptsAsync(int userId, string ticket, string terms);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserTermsOfUseService/UpdateInsertUserAccepts", ReplyAction="http://tempuri.org/IUserTermsOfUseService/UpdateInsertUserAcceptsResponse")]
        bool UpdateInsertUserAccepts(int userId, string terms, int version, System.DateTime acceptDate, string ticket);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserTermsOfUseService/UpdateInsertUserAccepts", ReplyAction="http://tempuri.org/IUserTermsOfUseService/UpdateInsertUserAcceptsResponse")]
        System.Threading.Tasks.Task<bool> UpdateInsertUserAcceptsAsync(int userId, string terms, int version, System.DateTime acceptDate, string ticket);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserTermsOfUseService/UpdateInsertTermsOfUse", ReplyAction="http://tempuri.org/IUserTermsOfUseService/UpdateInsertTermsOfUseResponse")]
        bool UpdateInsertTermsOfUse(string terms, int version, System.DateTime termsDate, string ticket);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserTermsOfUseService/UpdateInsertTermsOfUse", ReplyAction="http://tempuri.org/IUserTermsOfUseService/UpdateInsertTermsOfUseResponse")]
        System.Threading.Tasks.Task<bool> UpdateInsertTermsOfUseAsync(string terms, int version, System.DateTime termsDate, string ticket);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IUserTermsOfUseServiceChannel : Gro.Infrastructure.Data.UserTermsOfUseService.IUserTermsOfUseService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class UserTermsOfUseServiceClient : System.ServiceModel.ClientBase<Gro.Infrastructure.Data.UserTermsOfUseService.IUserTermsOfUseService>, Gro.Infrastructure.Data.UserTermsOfUseService.IUserTermsOfUseService {
        
        public UserTermsOfUseServiceClient() {
        }
        
        public UserTermsOfUseServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public UserTermsOfUseServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UserTermsOfUseServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UserTermsOfUseServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool CheckTerms(string terms, int version, string ticket) {
            return base.Channel.CheckTerms(terms, version, ticket);
        }
        
        public System.Threading.Tasks.Task<bool> CheckTermsAsync(string terms, int version, string ticket) {
            return base.Channel.CheckTermsAsync(terms, version, ticket);
        }
        
        public bool CheckUserAccepts(int userId, string ticket, string terms) {
            return base.Channel.CheckUserAccepts(userId, ticket, terms);
        }
        
        public System.Threading.Tasks.Task<bool> CheckUserAcceptsAsync(int userId, string ticket, string terms) {
            return base.Channel.CheckUserAcceptsAsync(userId, ticket, terms);
        }
        
        public bool UpdateInsertUserAccepts(int userId, string terms, int version, System.DateTime acceptDate, string ticket) {
            return base.Channel.UpdateInsertUserAccepts(userId, terms, version, acceptDate, ticket);
        }
        
        public System.Threading.Tasks.Task<bool> UpdateInsertUserAcceptsAsync(int userId, string terms, int version, System.DateTime acceptDate, string ticket) {
            return base.Channel.UpdateInsertUserAcceptsAsync(userId, terms, version, acceptDate, ticket);
        }
        
        public bool UpdateInsertTermsOfUse(string terms, int version, System.DateTime termsDate, string ticket) {
            return base.Channel.UpdateInsertTermsOfUse(terms, version, termsDate, ticket);
        }
        
        public System.Threading.Tasks.Task<bool> UpdateInsertTermsOfUseAsync(string terms, int version, System.DateTime termsDate, string ticket) {
            return base.Channel.UpdateInsertTermsOfUseAsync(terms, version, termsDate, ticket);
        }
    }
}
