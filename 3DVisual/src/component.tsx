import * as React from "react";
import { CustomVisual } from "./CustomVisual";

export interface State {
    url: string,
    error_message: string,
    loading: string,
    render_input: boolean,
    render_loading: boolean
}

export const initialState: State = {
    url: "",
    error_message: "",
    loading: "",
    render_input: true,
    render_loading: false
}

export class UploadForm extends React.Component <{}, State>{
    constructor(props) {
      super(props);
      this.state = initialState;  
      this.handleChange = this.handleChange.bind(this);
      CustomVisual.Instance.upload_form = this;
    }
  
    render() {
      const style_input_form: React.CSSProperties = { position: 'absolute', left: '50%', top: '50%', transform: 'translate(-50%, -50%)'};
      
      return (           
        <div className="App">    
            <div className="inputURL" style={style_input_form}>
                <form >
                {this.state.render_input ?
                <label>
                    URL:
                    <br/>
                    <input type="text" value={this.state.url} onChange={this.handleChange}/>
                </label>: null}     
                <br/>
                {this.state.render_input ?
                <button onClick={this.handleButtonClicked.bind(this)}> Submit
                </button>  : null}
                <br/>
                {this.state.render_input ?<label id="error_message_label">{this.state.error_message}</label>: null}
                </form>
            </div>
            <div className="loadingForm" style={style_input_form}>
                <form >
                {this.state.render_loading ?
                <label>
                    {"Loading... " + this.state.loading}
                </label>: null}
                </form>
            </div>
        </div>
        );
    }

    handleChange(e) {
        this.setState({
          url:e.target.value
        })
      }

    handleButtonClicked() {        
        CustomVisual.Load3DModel(this.state.url);
    }

    public componentWillMount() {
        UploadForm.updateCallback = (newState: State): void => { this.setState(newState); };
    }

    public componentWillUnmount() {
        UploadForm.updateCallback = null;
    }

    private static updateCallback: (data: object) => void = null;

    public static update(newState: State) {
        if(typeof UploadForm.updateCallback === 'function'){
            UploadForm.updateCallback(newState);            
        }
    }

    public static hide_input_form()
    {
                    this.update({
                    url: "",
                    error_message: "",
                    loading: "",
                    render_input: false,
                    render_loading: true
                });
    }

    public static hide_loading_form()
    {
                    this.update({
                    url: "",
                    error_message: "",
                    loading: "",
                    render_input: false,
                    render_loading: false
                });
                
    }

    public static update_loading_form(progress: string)
    {
                    this.update({
                    url: "",
                    error_message: "",
                    loading: progress,
                    render_input: false,
                    render_loading: true
                });
                
    }

    public static show_error_message(msg: string)
    {
                    this.update({
                    url: "",
                    error_message: msg,
                    loading: "",
                    render_input: true,
                    render_loading: false
                });
                
    }

}