import * as React from "react";
export interface State {
    url: string;
    error_message: string;
    loading: string;
    render_input: boolean;
    render_loading: boolean;
}
export declare const initialState: State;
export declare class UploadForm extends React.Component<{}, State> {
    constructor(props: any);
    render(): JSX.Element;
    handleChange(e: any): void;
    handleButtonClicked(): void;
    componentWillMount(): void;
    componentWillUnmount(): void;
    private static updateCallback;
    static update(newState: State): void;
    static hide_input_form(): void;
    static hide_loading_form(): void;
    static update_loading_form(progress: string): void;
    static show_error_message(msg: string): void;
}
