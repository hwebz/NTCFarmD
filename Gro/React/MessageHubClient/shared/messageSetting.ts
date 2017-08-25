import * as React from 'react';
import * as ReactDOM from 'react-dom';

export class MessageSetting {
    /**
     * Get pagesize for list message in Admin page 
     */
    getAdminPageSize() {
        var pageSizeElement = document.getElementById("administer_pagesize") as HTMLInputElement;
        if (pageSizeElement) {
            var pageSize = pageSizeElement.value;
            try {
                return parseInt(pageSize);
            } catch (ex) {
                return 0;
            }
        }
    }
   
   /**
    * Get pagesize for list message in User View Message page 
    */
    getUserPageSize() {
        var pageSizeElement = document.getElementById("user_pagesize") as HTMLInputElement;
        if (pageSizeElement) {
            var pageSize = pageSizeElement.value;
            try {
                return parseInt(pageSize);
            } catch (ex) {
                return 0;
            }
        }
    }

    /**
     * Get message displaying incase there is no message belonging to the selected category/messagetype 
     */
    getMessageForEmptyCategory(): string {
        var emptyMessage = document.getElementById("empty-category-message") as HTMLInputElement;
        if (emptyMessage) {
            return emptyMessage.value;
        }
    }

    /**
     * Get Url of User Setting Page
     */
    getUserSettingUrl() {
        var settingPageUrl = document.getElementById("setting-page-url") as HTMLInputElement;
        if (settingPageUrl) {
            return settingPageUrl.value;
        }
        return "";
    }

}

export var messageSetting = new MessageSetting(); 
