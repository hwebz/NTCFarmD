import {
    MessageType, MessageCategory,
    FreeMessageValidationResult, StandardMessageValidationResult
} from '../administerModels';

const noReceiverErrorMessage = "Du måste välja mottagare för meddelandet. Du kan välja mottagare från fil och/eller lägga till mottagares E-postadresser och mobilnummer manuellt";
const noCategoryErrorMessage = "Du måste välja meddelandeområde";
const noMessageTypeErrorMessage = "Du måste välja meddelandetyp";
const invalidSenderPhoneNumberErrorMessage = "Avsändarnumret är felaktig, korrigera detta";
const noTitleErrorMessage = "Du måste ange en rubrik för meddelandet.";

/**
 * check if an email has a valid format
 */
function validateEmail(email: string) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

function validatePhoneNumber(phone: string) {
    if (!phone) { return false; }
    var reduced = phone.trim();

    for (var i = 0; i < reduced.length; i++) {
        var c = reduced[i];
        if (i == 0 && c == "+") { continue; }
        if (i != 0 && c == " ") { continue; }

        if (isNaN(parseInt(c))) { return false; }
    }

    return true;
}

/**
 * validate the selected message category
 */
function validateMessageCategory(selectedCategory: MessageCategory): string[] {
    if (!selectedCategory) {
        return [noCategoryErrorMessage];
    }

    return [];
}

function validateMessageType(selectedType: MessageType): string[] {
    if (!selectedType) {
        return [noMessageTypeErrorMessage];
    }

    return [];
}

/**
 * validate the enter list of emails
 */
function validateReceivers(emails: string[], phones: string[], smsText: string): string[] {
    var errors = [];
    if (emails.length == 0 && phones.length == 0) {
        errors.push(`receivers:${noReceiverErrorMessage}`);
    }

    for (var email of emails) {
        if (validateEmail(email)) { continue; }
        errors.push("emails:Markerade E-posadresser är inkorrekta. Korrigera detta");
        break;
    }

    for (var phone of phones) {
        if (validatePhoneNumber(phone)) { continue; }
        errors.push("phones:Markerade telefonnummer är inkorrekta. Korrigera detta");
    }

    return errors;
}

/**
 * validate message body
 */
function validateMessageBody(title: string, body: string, sms: string): string[] {
    var errors: string[] = [];
    if (!title || title.length < 1) {
        errors.push(`title:${noTitleErrorMessage}`);
    }

    if ((!sms || !sms.trim()) && (!body)) {
        errors.push("body:Meddelandet kan inte vara tomt");
    }

    if (!!sms && sms.length > 160) {
        errors.push("sms:SMS- meddelande bör vara kortare än 160 tecken");
    }

    return errors;
}

/**
 * validate sms sender field
 */
function validateSmsSender(smsSender: string, smsReceivers: string[]): string[] {
    if (!smsSender || !smsSender.trim()) { return []; }
    return validatePhoneNumber(smsSender) ? [] : [invalidSenderPhoneNumberErrorMessage];
}

/**
 * validate the free message form and return list of errors
 */
function validateFreeMessageForm(
    selectedCategory: MessageCategory,
    emails: string[],
    phones: string[],
    smsSender: string,
    title: string,
    body: string,
    smsText: string
): FreeMessageValidationResult {
    var categoryErrors = validateMessageCategory(selectedCategory);
    var receiverErrors = validateReceivers(emails, phones, smsText);
    var smsSenderErrors = validateSmsSender(smsSender, phones);
    var bodyErrors = validateMessageBody(title, body, smsText);

    return {
        bodyErrors: bodyErrors,
        categoryErrors: categoryErrors,
        receiverErrors: receiverErrors,
        smsSenderErrors: smsSenderErrors
    };
}

function validateStandardMessageForm(
    selectedType: MessageType,
    emails: string[],
    phones: string[],
    smsSender: string,
    title: string,
    body: string,
    smsText: string,
    moreReceivers: boolean
): StandardMessageValidationResult {
    var typeErrors = validateMessageType(selectedType);
    var receiverErrors = moreReceivers ? validateReceivers(emails, phones, smsText) : [];
    var smsSenderErrors = validateSmsSender(smsSender, phones);
    var bodyErrors = validateMessageBody(title, body, smsText);

    return {
        bodyErrors: bodyErrors,
        receiverErrors: receiverErrors,
        smsSenderErrors: smsSenderErrors,
        typeErrors: typeErrors
    };
}

/**
 * try to validate and save a free message
 */
export function trySaveFreeMessage(
    selectedCategory: MessageCategory,
    emails: string[],
    phones: string[],
    smsSender: string,
    title: string,
    body: string,
    smsText: string
): Promise<number> {
    return new Promise<number>((resolve, reject) => {
        var validationResults = validateFreeMessageForm(selectedCategory, emails, phones, smsSender, title, body, smsText);
        if (validationResults.bodyErrors.length > 0
            || validationResults.categoryErrors.length > 0
            || validationResults.receiverErrors.length > 0
            || validationResults.smsSenderErrors.length > 0) {
            //there are errors
            reject(validationResults);
            return;
        }

        //fetch server
        var form = new FormData();
        form.append("AreaId", selectedCategory.CategoryId);
        form.append("EmailReceivers", emails.join(';'));
        form.append("SmsReceivers", phones.join(';'));
        form.append("SmsSender", smsSender);
        form.append("HeadLine", title);
        form.append("MailMessage", body);
        form.append("SmsMessage", smsText);

        fetch("/api/message-admin/save-adhoc-message", {
            method: "POST",
            credentials: 'same-origin',
            body: form
        }).then(r => {
            if (r.status != 200) {
                r.text().then(t => console.log(t));
                reject(`Request failed, the server returned ${r.status}`);
            }
            return r.json();
        }).then(r => {
            resolve(r["newId"]);
        });
    });
}

/**
 * try to validate and save a free message
 */
export function trySaveStandardMessage(
    selectedType: MessageType,
    emails: string[],
    phones: string[],
    smsSender: string,
    title: string,
    body: string,
    smsText: string,
    moreReceivers: boolean
): Promise<number> {
    return new Promise<number>((resolve, reject) => {
        var validationResults = validateStandardMessageForm(selectedType, emails, phones, smsSender, title, body, smsText, moreReceivers);
        if (validationResults.bodyErrors.length > 0
            || validationResults.typeErrors.length > 0
            || validationResults.receiverErrors.length > 0
            || validationResults.smsSenderErrors.length > 0) {
            //there are errors
            reject(validationResults);
            return;
        }

        //fetch server
        var form = new FormData();
        form.append("AreaId", selectedType.CategoryId);
        form.append("TypeId", selectedType.TypeId);
        form.append("SmsSender", smsSender);
        form.append("HeadLine", title);
        form.append("MailMessage", body);
        form.append("SmsMessage", smsText);
        if (moreReceivers) {
            form.append("EmailReceivers", emails.join(';'));
            form.append("SmsReceivers", phones.join(';'));
        }

        fetch("/api/message-admin/save-standard-message", {
            method: "POST",
            credentials: 'same-origin',
            body: form
        }).then(r => {
            if (r.status != 200) {
                r.text().then(t => console.log(t));
                reject(`Request failed, the server returned ${r.status}`);
            }
            return r.json();
        }).then(r => {
            resolve(r["newId"]);
        });
    });
}
