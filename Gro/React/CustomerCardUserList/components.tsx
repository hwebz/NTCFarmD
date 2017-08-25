import * as React from 'react';
import * as ReactDOM from 'react-dom';

export interface ComboBoxItem {
    name: string;
    value: string;
}

interface ComboboxProps {
    items: ComboBoxItem[];
    selectedItem: ComboBoxItem;

    //events
    onChange?: (value: string) => void;
}

interface ComboboxState {
    dropdownState: "open" | "closed";
}

export class Combobox extends React.Component<ComboboxProps, ComboboxState>{
    dropdown: HTMLUListElement;

    constructor(props) {
        super(props);
        this.state = {
            dropdownState: "closed"
        };
    }

    onSelection(ev: React.FormEvent<HTMLAnchorElement>, item: string) {
        ev.stopPropagation();
        this.dropdown.style.display = "none";

        if (this.props.onChange && this.props.selectedItem.value != item) {
            this.props.onChange(item);
        }
    }

    render() {
        var items = this.props.items.map(item => (
            <li data-value="Admin" key={item.value} style={{ position: "relative" }}>
                <a href="javascript:void(0)" className="user-profile"
                    onClick={e => this.onSelection(e, item.value)}> {item.name} </a>
            </li>
        ));

        return (
            <ul className="lm__form-dropdown type-3" style={{ width: "80%", marginTop: "10px" }}>
                <li className="showcase" onClick={e => this.dropdown.style.display = (this.dropdown.style.display == "none" ? "block" : "none")}>
                    <a >{this.props.selectedItem.name}</a>
                    <ul ref={r => this.dropdown = r} style={{
                        display: this.state.dropdownState == "open" ? "block" : "none",
                        overflow: "hidden",
                        overflowY: "visible",
                        maxHeight: "125px",
                        top: "44px"
                    }}>
                        {items}
                    </ul>
                </li>
            </ul>
        );
    }
}
