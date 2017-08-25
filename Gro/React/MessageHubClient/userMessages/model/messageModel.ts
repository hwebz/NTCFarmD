import { DeliveryItem } from './deliveryModel';

export interface MessageSimple {
    id: number;
}

export interface Message extends MessageSimple {
    isSelected: boolean;
    categoryId: number;
    categoryName: string;
    header: string;
    content: string;
    receivedDate: string;
    isUnRead: boolean;
    isStarred: boolean;
    isTrash: boolean;
    isDelete: boolean;
    customerAddress: string;
    customerName: string;
    customerZipAndCity: string;
    // deliveryItems: DeliveryItem[],
    messageTable: MessageTable,
    messageType: number,
    deliveriesView:any
}
export interface MessageTable {
    carrier: string,
    carNo: string,
    mobileNo: string,
    freightNo: string,
    headerItems: string[],
    firstTblHeaders:string[],
    deliveryRows: MessageRow[],    
    firstTblRows: MessageRow[],
    secondHeaderItems: string[],
    secondTableRows: MessageRow[],
}
export interface MessageRow {
    items: any[]
}
