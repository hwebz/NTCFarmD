import * as React from "react";
const bankidLink =  document.getElementById("LoginBankAccountUrl")["value"];

export var BankIdLogin = (props) => !!window["serialNumber"] ? null : (
    <div className="author-inform-form__input">
        <div className="bankid-block">
            <img src="/Static/dist/images/bank_id.svg" alt="BankID Logo" className="bank-id__logo" />
            <p>Du måste vara inloggad med BankID för att kunna använda tjänsten.</p>
            <a className="bank-id__btn lm__form-btn reverse-state" target="_self"
                href={bankidLink} >Logga in med BankID</a>
        </div>
    </div>
);
