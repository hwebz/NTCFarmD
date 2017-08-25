export interface SettingRow {
    id: number;
    mailId: number;
    showMail: boolean;
    mailChecked: boolean;
    smsId: number;
    showSMS: boolean;
    smsChecked: boolean;
    name: string;
    categoryId: number;
    tab: number;
}

export interface SettingCategory {
    tab: number;
    categoryName: string;
    categoryId: number;
}

export interface SettingTab {
    displayName: string;
    displayOrder: number;
}

export interface SettingItemChangeEvent {
    item: SettingRow;
    areaType: string;
}

export interface SaveSettingModel {
    CustomerOrgId: number;
    MessageSettingsId: number;
    ModeOfDeliveryId: number;
    MsgAreaId: number;
    MsgTypeId: number;
    UserID: number;
    Value: boolean;
}
