import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { SettingRow } from './settingModels';

interface SettingItemProps {
    item: SettingRow;

    //events
    mailChange: (newValue: boolean) => void;
    smsChange: (newValue: boolean) => void;
}

export class SettingItemRow extends React.Component<SettingItemProps, any> {
    constructor(props: SettingItemProps) {
        super(props);
    }

    shouldComponentUpdate(nextProps: SettingItemProps, nextState: any) {
        var yes = this.props.item != nextProps.item;
        return yes;
    }

    smsChanged() {
        var newValue = !this.props.item.smsChecked;
        this.props.smsChange(newValue);
    }

    epostChanged() {
        var newValue = !this.props.item.mailChecked;
        this.props.mailChange(newValue)
    }

    render() {
        var smsClass = "lm__checkbox lm__tick " + (this.props.item.showSMS ? "" : "hide");
        var mailClass = "lm__checkbox lm__tick " + (this.props.item.showMail ? "" : "hide");
        return (
            <tr>
                <td style={{ borderRight: 0 }}>{this.props.item.name}</td>
                <td style={{
                    borderLeftWidth: this.props.item.showSMS ? 1 : 0,
                    borderRightWidth: this.props.item.showSMS ? 1 : 0,
                }}>
                    <div className={smsClass}>
                        <input type="checkbox" checked={this.props.item.smsChecked}
                            onChange={_ => this.smsChanged()} />
                        <label></label>
                    </div>
                </td>
                <td style={{
                    borderLeftWidth: this.props.item.showMail ? 1 : 0
                }}>
                    <div className={mailClass}>
                        <input type="checkbox" checked={this.props.item.mailChecked}
                            onChange={_ => this.epostChanged()} />
                        <label></label>
                    </div>
                </td>
            </tr>
        );
    }
}
