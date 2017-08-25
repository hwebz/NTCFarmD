import * as React from 'react';
import { isMobile } from '../../shared/device-detect';

var $ = window["$"];

interface RichEditorProps {
    extraClass?: string;
}

export class RichEditor extends React.Component<any, any>{
    editor: any;
    jQueryEditorElement: any;
    textArea: HTMLTextAreaElement;
    isMobile: boolean;

    constructor() {
        super();
        this.state = {
            value: ""
        };

        this.isMobile = isMobile();
    }

    componentDidMount() {
        if (this.isMobile) {
            return;
        }

        this.jQueryEditorElement = $('#editor');
        $["trumbowyg"].svgPath = '/Static/dist/scripts/ui/icons.svg';
        window["editor"] = this.jQueryEditorElement;
        //this.jQueryEditorElement.svgPath = 'https://cdnjs.cloudflare.com/ajax/libs/Trumbowyg/2.3.0/ui/icons.svg';
        this.jQueryEditorElement.trumbowyg({
            fullscreenable: false,
            lang: 'sv'
        });

        this.jQueryEditorElement.trumbowyg('html', this.props.body);
    }

    getMarkup() {
        var markup = this.getMarkupValue();
        var virtualDom = document.createElement("html");
        virtualDom.innerHTML = markup;
        var allLinks = virtualDom.getElementsByTagName("a");
        for (var i = 0; i < allLinks.length; i++) {
            let link = allLinks[i];
            let href = link.attributes['href'].nodeValue;
            if (!href) { continue; }
            if (href.startsWith("http://") || href.startsWith("https://")) { continue; }

            allLinks[i]["href"] = `http://${href}`;
        }
        var innerHTML = virtualDom.getElementsByTagName("body")[0].innerHTML;
        return innerHTML;
    }

    getMarkupValue(): string {
        if (this.isMobile) {
            return this.textArea.value;
        }

        return !this.jQueryEditorElement ? "" : this.jQueryEditorElement.trumbowyg('html');
    }

    render() {
        var editor = this.isMobile ? (
            <textarea name="epost-meddelande" className="epost-meddelande" required={true}
                placeholder="Skriv ett meddelande"
                ref={r => this.textArea = r}></textarea>
        ) : (
                <div id="editor" >
                </div>
            );

        return editor;
    }
}
