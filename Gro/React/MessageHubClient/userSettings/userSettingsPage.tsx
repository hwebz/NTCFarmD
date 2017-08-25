import * as React from 'react';
import { SettingRow, SettingCategory, SettingTab } from './settingModels';
import { SettingItemRow } from './settingItemRow';
import { SavingSuccessModal } from './successModal';
import { getInitialSettings, saveMessageSettings, resetSettings, updateMailSetting, updateSmsSetting, calculateSettingsToSave } from './action';
import "./userSettingsPage.scss";
import { GeneralSettingTable } from './generalSettingTable';
import * as Immutable from 'immutable';
import { store } from './store';

interface UserSettingPageState {
    tabs: Immutable.Map<number, SettingTab>;
    categories: Immutable.Map<number, SettingCategory>;
    rows: Immutable.List<SettingRow>;
    currentTab?: number;
    editState: string;
}

export class UserSettingPage extends React.Component<any, UserSettingPageState> {
    editState = "editing";

    constructor() {
        super();

        this.state = {
            tabs: null,
            categories: null,
            rows: null,
            editState: "editing"
        };
    }

    resetState = () => {
        this.setState({
            tabs: store.tabs(),
            categories: store.categories(),
            rows: store.rows(),
            editState: this.editState,
            //hard code tab for now
            currentTab: 0
        });
    }

    saveSuccess = () => {
        this.editState = "saved";
        this.resetState();
    }

    beforeUnload = (e: BeforeUnloadEvent) => {
        var settingsToSave = calculateSettingsToSave();
        if (settingsToSave.length > 0) {
            const message = "Vill du verkligen lämna den här sidan utan att ha sparat ändringar?";
            if (!!e) {
                e.returnValue = message;
            }
            return message;
        }
    }

    componentDidMount() {
        window.addEventListener("beforeunload", this.beforeUnload);
        store.addListener("change", this.resetState);
        store.addListener("saveSuccess", this.saveSuccess);
        getInitialSettings();
    }

    componentWillUnmount() {
        store.removeListener("change", this.resetState);
        store.removeListener("saveSuccess", this.saveSuccess);
    }

    settingItemSmsChange(newValue: boolean, item: SettingRow, categoryId: number) {
        updateSmsSetting(newValue, item, categoryId);
    }

    settingItemMailChange(newValue: boolean, item: SettingRow, categoryId: number) {
        updateMailSetting(newValue, item, categoryId);
    }

    resetButton_click() {
        resetSettings();
    }

    saveButton_click() {
        this.editState = "saving";
        this.resetState();
        saveMessageSettings();
    }

    successModalCloseButton_click() {
        this.editState = "editing";
        this.resetState();
    }

    render() {
        var agricultureAreas = this.getAgricultureSettingsRendering();
        var successModal: JSX.Element = null;
        if (this.state.editState != "editing") {
            successModal = <SavingSuccessModal confirmClick={() => this.successModalCloseButton_click()}
                state={this.state.editState} />;
        }

        return (
            <div className="lm__block meddelanden-block user-settings-page">
                <p className="first-summary">
                    Här kan du göra aviseringsinställningar för meddelanden från Lantmännen Lantbruk och Lantmännen Maskin. Alla typer
                            av meddelanden skickas automatiskt till meddelandefunktionen i LM2 oavsett dina inställningar här.
                        </p>
                <form method="POST">

                    <div className="lantbruk-block">
                        <h3>Meddelanden från Lantmännen</h3>
                        {agricultureAreas}
                    </div>
                    <div className="author-inform-form__input align-right-text block-button-on-mobile two-buttons">
                        <input type="button" className="lm__form-btn left-button" value="Återställ"
                            onClick={e => this.resetButton_click()} style={{ marginRight: "5px", marginBottom: "5px" }} />
                        <input type="button" className="lm__form-btn reverse-state" value="Uppdatera"
                            onClick={e => this.saveButton_click()} />
                    </div>
                </form>
                {successModal}
            </div>
        );
    }

    getAgricultureSettingsRendering() {
        if (!this.state.tabs || !this.state.categories || !this.state.rows) {
            return null;
        }
        var categoryAreas = this.state.categories
            //.filter(c => c.tab == this.state.currentTab)
            .map(category => {
                let smsHeaderVisible = this.state.rows
                    .filter(r => r.categoryId == category.categoryId && r.showSMS == true).count() > 0;
                let emailHeaderVisible = this.state.rows
                    .filter(r => r.categoryId == category.categoryId && r.showMail == true).count() > 0;

                var rows = this.state.rows
                    .filter(r => r.categoryId == category.categoryId)
                    .map(r => {
                        var key = `${r.id}_${r.categoryId}`;
                        return <SettingItemRow key={key} item={r}
                            smsChange={nv => this.settingItemSmsChange(nv, r, category.categoryId)}
                            mailChange={nv => this.settingItemMailChange(nv, r, category.categoryId)} />;
                    })
                    .toArray();

                return (
                    <div key={category.categoryId} >
                        <table >
                            <thead>
                                <tr>
                                    <th>{category.categoryName}</th>
                                    <th>{smsHeaderVisible ? "SMS" : ""}</th>
                                    <th>{emailHeaderVisible ? "E-Post" : ""}</th>
                                </tr>
                            </thead>
                            <colgroup>
                                <col width="54%" />
                                <col width="23%" />
                                <col width="23%" />
                            </colgroup>
                            <tbody>
                                {rows}
                            </tbody>
                        </table>
                    </div >
                );
            })
            .toArray();

        return categoryAreas;
    }
}
