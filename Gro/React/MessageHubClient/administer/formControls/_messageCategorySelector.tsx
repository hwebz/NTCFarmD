import * as React from 'react';
import {MessageCategory} from '../administerModels';

interface MessageCategorySelectorProps {
    categories: MessageCategory[];

    errors: string[];
}

export class MessageCategorySelector extends React.Component<MessageCategorySelectorProps, any>{
    select: HTMLSelectElement;

    constructor(props) {
        super(props);
        if (!this.props.categories || this.props.categories.length == 0) {
            throw "MessageCategorySelector must have a default option";
        }
    }

    getSelectedCategory() {
        if (this.select.selectedIndex == 0) { return null; }
        return this.props.categories[this.select.selectedIndex];
    }

    render() {
        var options = this.props.categories.map(c => {
            return (
                <option key={c.CategoryId} value={c.CategoryId.toString()} >{c.CategoryName}</option>
            );
        });

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
                <select ref={r => this.select = r} className={selectElementClass}>
                    {options}
                </select>
                {errors}
            </div>
        );
    }
}
