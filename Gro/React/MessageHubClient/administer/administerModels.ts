export interface MessageCategory {
    CategoryName: string;
    CategoryId: number;
    Types: MessageType[];
}

export interface MessageType {
    TypeName: string;
    TypeId: number;
    CategoryId: number;
}

export interface MessageItem {
    AreaDescription: string;
    Handled: boolean;
    HeadLine: string;
    SendDate: Date;
    MailMessage: string;
    MessageArea: number;
    MessageId: number;
    MessageType: number;
    MessageRead: boolean;
    ModeOfDelivery: number;
    Status: number;
    TypeDescription: string;
    MsgText: string;
    ValidFrom: Date;
    ValidTo: Date;
}

export interface MessageExtendedInfo {
    CarNo: string;
    Carrier: string;
    Container: string;
    DeliveredQuantity: number;
    DeliveryDate: Date;
    FreightNo: string;
    ItemName: string;
    MessageId: number;
    MobileNo: string;
    OrderLine: number;
    OrderNo: string;
    OrderQuantity: number;
    PlannedDeliveryDate: Date;
    Unit: string;
    Warehouse: string;
}

export interface MessageDetail {
    Message: MessageItem;
    ExtendedInfo: MessageExtendedInfo[];
    receivers: string[]
}

export interface FreeMessageValidationResult {
    categoryErrors: string[];
    receiverErrors: string[];
    smsSenderErrors: string[];
    bodyErrors: string[];
}

export interface StandardMessageValidationResult {
    typeErrors: string[];
    receiverErrors: string[];
    smsSenderErrors: string[];
    bodyErrors: string[];
}
