import { SettingRow, SettingCategory, SettingTab, SaveSettingModel } from './settingModels';
import { store } from './store';
import "isomorphic-fetch"

const getMessageSettingsEndpoint = "/api/user/message-settings";
const saveMessageSettingsEndpoint = "/api/user/save-settings";

var loading = false;

function getSettings(): Promise<any> {
    var headers = new Headers();
    headers.append('pragma', 'no-cache');
    headers.append('cache-control', 'no-cache');

    var p = fetch(getMessageSettingsEndpoint, {
        credentials: 'same-origin',
        cache: "no-cache",
        headers: headers
    }).then(r => {
        if (r.status != 200) {
            console.log(`cannot get settings from server: ${r.status}/${r.statusText}`);
            return null;
        }
        return r.json();
    }).then(json => {
        return json;
    });

    return p;
}

function clone<T>(source: T): T {
    var dest = {};
    for (var key in source) {
        dest[key as string] = source[key];
    }

    return dest as T;
}

/**
 * Get the user's saved settings 
 */
export function getInitialSettings() {
    loading = true;
    var settingRows: SettingRow[] = [];
    var settingTabs: SettingTab[] = [];
    var settingCategories: SettingCategory[] = [];
    getSettings().then(r => {
        loading = false;
        if (!r) {
            return;
        }
        for (var tab of r) {
            settingTabs.push({
                displayName: tab["DisplayName"],
                displayOrder: tab["DisplayOrder"]
            });

            for (var category of tab["Category"]) {
                if (category.CategoryName == "Alla meddelandetyper") {
                    continue;
                }
                settingCategories.push({
                    categoryId: category["Categoryid"],
                    categoryName: category["CategoryName"],
                    tab: tab["DisplayOrder"]
                });

                for (var setting of category["MessageType"]) {
                    settingRows.push({
                        categoryId: category["Categoryid"],
                        id: setting["Id"],
                        mailChecked: setting["MailChecked"],
                        mailId: setting["MailId"],
                        name: setting["Name"],
                        showMail: setting["ShowMail"],
                        showSMS: setting["ShowSms"],
                        smsChecked: setting["SmsChecked"],
                        smsId: setting["SmsId"],
                        tab: tab["DisplayOrder"]
                    });
                }
            }
        }
        store.setInitialSetting(settingTabs, settingCategories, settingRows);
    });
}

export function calculateSettingsToSave(): Array<SaveSettingModel> {
    var initialSettings = store.initialSettingItems;
    var currentSettings = store.rows();
    var settingsToSave = new Array<SaveSettingModel>();

    for (var i = 0; i < initialSettings.size; i++) {
        if (initialSettings.get(i) != currentSettings.get(i)) {
            var currentSetting = currentSettings.get(i);

            //change sms setting
            settingsToSave.push({
                CustomerOrgId: 0,
                UserID: 0,
                Value: currentSetting.smsChecked,
                MessageSettingsId: currentSetting.smsId,
                ModeOfDeliveryId: currentSetting.smsChecked ? 2 : 0,
                MsgAreaId: currentSetting.categoryId,
                MsgTypeId: currentSetting.id
            });

            //chane email setting
            settingsToSave.push({
                CustomerOrgId: 0,
                UserID: 0,
                Value: currentSetting.mailChecked,
                MessageSettingsId: currentSetting.mailId,
                ModeOfDeliveryId: currentSetting.mailChecked ? 1 : 0,
                MsgAreaId: currentSetting.categoryId,
                MsgTypeId: currentSetting.id
            });
        }
    }

    return settingsToSave;
}

/**
 * Save the user's settings
 */
export function saveMessageSettings() {
    if (loading) { return; }

    var settingsToSave = calculateSettingsToSave();
    if (settingsToSave.length == 0) {
        store.saveSuccess();
        return;
    }

    var headers = new Headers();
    headers.set('Content-Type', 'application/json; charset=utf-8');

    fetch(saveMessageSettingsEndpoint, {
        method: "POST",
        credentials: 'same-origin',
        headers: headers,
        body: JSON.stringify(settingsToSave)
    }).then(r => {
        if (r.status == 200) {
            return r.json();
        }
        throw r.text();
    }).then(v => {
        if (v) {
            //save successful
            store.saveSuccess();
            getInitialSettings();
        } else {
            alert("Save failed");
        }
    });
}

/**
 * Reset the settings to the initial values
 */
export function resetSettings() {
    store.resetSettings();
}

/**
 * Change sms setting of a setting item
 */
export function updateSmsSetting(newValue: boolean, item: SettingRow, categoryId: number) {
    var data = clone(item);
    data.smsChecked = newValue;
    store.updateRow(data.id, data, categoryId);
}

/**
 * Change mail setting of a setting item
 */
export function updateMailSetting(newValue: boolean, item: SettingRow, categoryId: number) {
    var data = clone(item);
    data.mailChecked = newValue;
    store.updateRow(data.id, data, categoryId);
}
