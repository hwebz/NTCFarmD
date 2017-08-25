import * as React from 'react';

interface GeneralSettingTableProps {
    agricultureSms: boolean;
    agricultureEpost: boolean;
    machineSms: boolean;
    machineEpost: boolean;

    //events
    /**
     * general setting change event
     * @param type 'agriculture' or 'machine'
     * @param option 'sms' or 'epost'
     * @param value new value: 'true' or 'false'
     */
    onSettingChange: (type: string, option: string, value: boolean) => void;
}

export class GeneralSettingTable extends React.Component<GeneralSettingTableProps, any>{

    shouldComponentUpdate(nextProps: GeneralSettingTableProps, nextState: any) {
        return nextProps.agricultureEpost != this.props.agricultureEpost
            || nextProps.agricultureSms != this.props.agricultureSms
            || nextProps.machineEpost != this.props.machineEpost
            || nextProps.machineSms != this.props.machineSms;
    }

    agricultureSms_change() {
        var newValue = !this.props.agricultureSms;
        this.props.onSettingChange('agriculture', 'sms', newValue);
    }

    agricultureEpost_change() {
        var newValue = !this.props.agricultureEpost;
        this.props.onSettingChange('agriculture', 'epost', newValue);
    }

    machineSms_change() {
        var newValue = !this.props.machineSms;
        this.props.onSettingChange('machine', 'sms', newValue);
    }

    machineEpost_change() {
        var newValue = !this.props.machineEpost;
        this.props.onSettingChange('machine', 'epost', newValue);
    }

    render() {
        return (
            <table >
                <thead>
                    <tr>
                        <th>Jag vill ha: </th>
                        <th>SMS</th>
                        <th>E-Post</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>Avisering från Lantmännen Lantbruk</td>
                        <td>
                            <div className="lm__checkbox lm__switcher">
                                <input name="agricultureSms" type="checkbox"
                                    checked={this.props.agricultureSms}
                                    onChange={e => this.agricultureSms_change()} />
                                <label></label>
                            </div>
                        </td>
                        <td>
                            <div className="lm__checkbox lm__switcher">
                                <input name="agricultureEPost" type="checkbox"
                                    checked={this.props.agricultureEpost}
                                    onChange={e => this.agricultureEpost_change()} />
                                <label></label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>Avisering från Lantmännen Maskin</td>
                        <td>
                            <div className="lm__checkbox lm__switcher">
                                <input name="machineSms" type="checkbox"
                                    checked={this.props.machineSms}
                                    onChange={e => this.machineSms_change()} />
                                <label></label>
                            </div>
                        </td>
                        <td>
                            <div className="lm__checkbox lm__switcher">
                                <input name="machineEPost" type="checkbox"
                                    checked={this.props.machineEpost}
                                    onChange={e => this.machineEpost_change()} />
                                <label></label>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        );
    }
}
