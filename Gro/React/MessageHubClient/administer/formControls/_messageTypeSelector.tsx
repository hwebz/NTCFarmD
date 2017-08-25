import * as React from 'react';
import {MessageType, MessageCategory} from '../administerModels';

interface MessageTypeSelectorProps {
    categories: MessageCategory[];
    errors: string[]
}

export class MessageTypeSelector extends React.Component<MessageTypeSelectorProps, any>{
    select: HTMLSelectElement;

    constructor(props) {
        super(props);
        if (!this.props.categories || this.props.categories.length == 0) {
            throw "MessageTypeSelector must have a default option";
        }
    }

    getSelectedType() {
        if (this.select.selectedIndex == 0) { return null; }

        var selectedValue = this.select.value;
        var ids = selectedValue.split('_').map(id => parseInt(id));
        var selectedCategory = this.props.categories.filter(c => c.CategoryId == ids[0])[0];
        var selectedType = selectedCategory.Types.filter(t => t.TypeId == ids[1])[0];
        return selectedType;
    }

    getSelectOptions() {
        var elements: JSX.Element[] = [];
        for (var category of this.props.categories) {
            elements.push(
                <option style={{ fontWeight: "bold" }} key={category.CategoryId.toString() } value={category.CategoryId.toString()} disabled={true}>
                    {category.CategoryName}
                </option>
            );

            for (var type of category.Types) {
                var typeKey = `${category.CategoryId}_${type.TypeId}`;
                elements.push(
                    <option key={typeKey} value={typeKey} style={{ marginLeft: "5px" }}>&nbsp; &nbsp; &nbsp; {type.TypeName}</option>
                );
            }
        }

        return elements;
    }

    render() {
        var options = this.getSelectOptions();
        var errors: JSX.Element[] = [];
        for (var idx = 0; idx < this.props.errors.length; idx++) {
            var err = this.props.errors[idx];
            errors.push(<br key={idx.toString() + "br"}/>);
            errors.push(
                <span className="error-item" key={idx.toString() }>{err}</span>
            );
        }

        var selectElementClass = "lm__form-dropdown";
        if (this.props.errors.length > 0) {
            selectElementClass += " rubrik error";
        }

        return (
            <div className="meddelande-types dropdown-selector">
                <h3>Välj typ av meddelande*</h3>
                <select ref={r => this.select = r} className={selectElementClass} defaultValue={this.props.categories[0].CategoryId.toString()}>
                    {options}
                </select>
                {errors}
            </div>
        );
    }
}
