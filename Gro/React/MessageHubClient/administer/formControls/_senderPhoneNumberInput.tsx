import * as React from 'react';

export interface SenderPhoneNumberInputProps {
    errors: string[];
}

export class SenderPhoneNumberInput extends React.Component<SenderPhoneNumberInputProps, any>{
    input: HTMLInputElement;

    getValue() {
        return this.input.value;
    }

    render() {
        var errors: JSX.Element[] = [];
        for (var idx = 0; idx < this.props.errors.length; idx++) {
            var err = this.props.errors[idx];
            errors.push(<br key={idx.toString() + "br"} />);

            errors.push(
                <span className="error-item" key={idx.toString()}>{err}</span>
            ); 
        }

        return (
            <div >
                <h3>Avsändare - SMS</h3>
                <p>Om du vill att mottagare av SMS-meddelande ska kunna svara kan du skriva in mobilnummer för detta i fältet nedan.Om fältet lämnas tomt visas namnet på företaget som avsändare.</p>
                <input type="tel" name="phone_number" className="phone_number" placeholder="+46712345678"
                    ref={r => this.input = r} />

                {errors}
            </div>
        );
    }
}
