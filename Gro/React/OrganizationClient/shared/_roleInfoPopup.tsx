import * as React from 'react';

export class InfoPopup extends React.Component<{ onclose: () => void }, any>{
    protected getChildren(): JSX.Element {
        return null;
    }

    render() {
        return (
            <div className="lm__information-modal__wrapper">
                <div className="mask"></div>
                <div className="lm__information-modal">
                    {this.props.children}
                    <div className="lm__information-modal__close-btn">
                        <a style={{ cursor: "pointer" }} onClick={() => this.props.onclose()}></a>
                    </div>
                </div>
            </div>
        );
    }
}

export var BasfunktionerInfo = (
    <div>
        <h3 className="lm__information-modal__title">Basfunktioner</h3>
        <p>Tillgång till grundläggande funktionalitet i portalen, till exempel nyheter, marknadsrapporter och produktkatalog</p>
    </div>
);

export var Prenumeration = (
    <div >
        <h3 className="lm__information-modal__title">Prenumerationer - Rådgivning</h3>
        <strong>Behörighet - Kan se</strong>
        <p>Växline Syd, VäxtlineVäst, Växtline Vallbrev. Växtodlingsbrev, Utsädesodlare brev, Kalkyler</p>
        <p>Vilka prenumerationer användaren kommer kunna se beroer på vilka prenumerationer som är tecknade för detta KundID. </p>
        <strong>Fullständig behörighet</strong>
        <p>Se-behörighet, Teckna prenumeration</p>
    </div>
);

export var SpannmalsAvtal = (
    <div >
        <h3 className="lm__information-modal__title">Spannmålsavtal</h3>
        <strong>Behörighet - Kan se</strong>
        <p>Prisbevakning spot & terminsavtal. Depå-, tork- och poolavtal. Pool & spannmålspris, Prisutveckling spannmål, Mina spannmålsavtal</p>
        <strong>Fullständig behörighet</strong>
        <p>Se-behörighet. Att skapa prisbevakning spot & terminsavtal, Att säkra depåavtal, Att teckna depå-, tork- och poolavtal</p>
    </div>
);

export var Leveransinfo = (
    <div>
        <h3 className="lm__information-modal__title">Leveransinformation</h3>
        <strong>Behörighet - Kan se</strong>
        <p>Leveransförsäkran, söka transport, tidsbokning för gårdshämtning, mottagaranläggnignar, prenumerationer av öppettider, invägningar</p>
        <strong>Fullständig behörighet</strong>
        <p>Se-behörighet, Lämna leveransförsäkran, Boka tid för gårdshämtning</p>
    </div>
);

export var Bestallning = (
    <div>
        <h3 className="lm__information-modal__title">Beställning (E-handel)</h3>
        <strong>Behörighet - Kan se</strong>
        <p>Gjorda beställningar från E.handel Maskin och E-handel Lantbruk</p>
        <strong>Fullständig behörighet</strong>
        <p>Tillgång till E-handel Maskin,Tillgång till E-handel Lantbruk.</p>
    </div>
);

export var Maskin = (
    <div>
        <h3 className="lm__information-modal__title">Maskiner</h3>
        <strong>Behörighet - Kan se</strong>
        <p>Maskiner kopplade till företaget. </p>
        <strong>Fullständig behörighet</strong>
        <p>Lägga till Maskin, Ta bort Maskin, Boka service.</p>
    </div>
);

export var Ekonomi = (
    <div>
        <h3 className="lm__information-modal__title">Ekonomi</h3>
        <strong>Behörighet - Kan se</strong>
        <p>Saldo, Kreditpost, Faktura, Betald faktura, Kontobesked, Emissionsinsatser, Räntesatser,  Kontakt </p>
        <strong>Fullständig behörighet</strong>
        <p>Se-behörighet, Beställa kopia, till Lantmännenkortet, Att överföra Saldo, Att överföra Kreditpost, Att betala faktura, Att göra utbetalning, Att anmäla nytt konto, E-handel (Lantbruk & Maskin)</p>
    </div>
);
