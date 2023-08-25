import { Component } from "react";
import '../../fonts.css';
import AITag from "../dialogs/AITag/AITag";
import AOTag from "../dialogs/AOTag/AOTag";
import { NavMenu } from "../Nav/NavMenu";
import './DatabaseManager.css';
import TagService from "../../services/TagService";


export class DatabaseManager extends Component {

    constructor(props) {
        super(props);
        this.state = {
            isDropdownOpen: false,
            selectedItem: null,
            AOData: [],
            DOData: [],
            AIData: [],
            DIData: [],
            isDO: false,
            isDI: false,
        };
    }

    toggleOutput = () => {
        this.setState(prevState => ({
            isDO: !prevState.isDO, 
        }));
    };

    toggleInput = () => {
        this.setState(prevState => ({
            isDI: !prevState.isDI, 
        }));
    };

    toggleValue = (itemId) => {
        // TODO Implementirati logiku za promenu isScanning atributa sa dati itemId
    }

    toggleDropdown = () => {
        this.setState((prevState) => ({
            isDropdownOpen: !prevState.isDropdownOpen,
        }));
    };

    openDialog = (selectedItem) => {
        this.setState({
            selectedItem: selectedItem,
        });
    };

    closeDialog = () => {
        this.setState({
            selectedItem: null,
        });
    };

    async componentDidMount() {
        const AOData = await TagService.getAOData(); 
        const DOData = await TagService.getDOData(); 
        const AIData = await TagService.getAIData();
        const DIData = await TagService.getDIData();
        console.log(DIData)
        this.setState({ AOData, DOData, AIData, DIData });
    }


    render() {
        const { isDropdownOpen, selectedItem, isDO, isDI, AOData, DOData, AIData, DIData } = this.state;

        return (
            <div>
                <NavMenu showNavbar={true} />
                <h1 id="tableLabel">Database Manager</h1>
                {/*<img alt="." src="../..images/plus.png"/>*/}

                <p id="add-tag" onClick={this.toggleDropdown}>Add tag</p>

                {isDropdownOpen && (
                    <div id="dropdown">
                        <ul id="dropdown-tags">
                            <li onClick={() => this.openDialog("AO")} className="tag bottom-margin">ANALOG OUTPUT</li>
                            <li onClick={() => this.openDialog("AI")} className="tag bottom-margin">ANALOG INPUT</li>
                            <li onClick={() => this.openDialog("DO")} className="tag bottom-margin">DIGITAL OUTPUT</li>
                            <li onClick={() => this.openDialog("DI")} className="tag">DIGITAL INPUT</li>
                        </ul>
                    </div>
                )}

                {/*Dialogs */}
                {selectedItem === "AI" && <AITag onClose={this.closeDialog} />}
                {selectedItem === "AO" && <AOTag onClose={this.closeDialog} />}

                <div id="output-container">
                    <div className="header">
                        <h3>Outputs</h3>
                        <div className={`toggle-switch ${isDO ? 'on' : ''}`} onClick={this.toggleOutput}>
                            <div className="toggle-slider"></div>
                            <div className={`toggle-text digital ${isDO ? '' : 'active'}`}>Digital</div>
                            <div className={`toggle-text analog ${isDO ? 'active' : ''}`}>Analog</div>
                        </div>
                    </div>
                    <div className="object-list">
                        {isDO ? (
                            AOData.map(item => (
                                <div key={item.id} className="output-tag">
                                    <div className="tag-description">
                                        <h6 style={{ float: 'left' }}>{item.tagName}</h6>
                                        <p className="attribute" style={{ margin: "0px" }}>Description: {item.description}</p>
                                        <p className="attribute" style={{ margin: "0px" }}>Address: {item.address}</p>
                                        <p className="attribute">Range: ({item.lowLimit},{item.highLimit})</p>
                                    </div>
                                    <p className="value">{item.value} {item.units}</p>
                                    <div className="edit-delete-icons">
                                        <img src="/images/delete.png" alt="Delete" className="icon" />
                                        <img src="/images/pencil.png" alt="Edit" className="icon" />
                                    </div>
                                </div>
                            ))
                        ) : (
                            DOData.map(item => (
                                <div key={item.id} className="output-tag">
                                    <div className="tag-description">
                                        <h6 style={{ float: 'left' }}>{item.tagName}</h6>
                                        <p className="attribute" style={{ margin: "0px" }}>Description: {item.description}</p>
                                        <p className="attribute">Address: {item.address}</p>
                                    </div>
                                    <p className="value">{item.value === 0 ? 'Off' : 'On'}</p>
                                    <div className="edit-delete-icons">
                                        <img src="/images/delete.png" alt="Delete" className="icon" />
                                        <img src="/images/pencil.png" alt="Edit" className="icon" />
                                    </div>
                                </div>
                            ))
                        )}
                    </div>
                </div>

                <div id="input-container">
                    <div className="header">
                        <h3>Inputs</h3>
                        <div className={`toggle-switch ${isDI ? 'on' : ''}`} onClick={this.toggleInput}>
                            <div className="toggle-slider"></div>
                            <div className={`toggle-text digital ${isDI ? '' : 'active'}`}>Digital</div>
                            <div className={`toggle-text analog ${isDI ? 'active' : ''}`}>Analog</div>
                        </div>
                    </div>
                    <div className="object-list">
                        {isDI ? (
                            AIData.map(item => (
                                <div key={item.id} className="output-tag">
                                    <div className="tag-description">
                                        <h6 style={{ float: 'left' }}>{item.tagName}</h6>
                                        <p className="attribute" style={{ margin: "0px" }}>Description: {item.description}</p>
                                        <p className="attribute" style={{ margin: "0px" }}>Address: {item.address}</p>
                                        <p className="attribute" style={{ margin: "0px" }}>Range: ({item.lowLimit},{item.highLimit})</p>
                                        <p className="attribute" style={{ margin: "0px" }}>Scan Time: {item.scanTime} ms</p>
                                        <p className="attribute">Units: {item.units}</p>
                                    </div>
                                    <p className="value">{item.driver === 0 ? 'SIMULATION' : 'RTU'}</p>
                                    <div className="edit-delete-icons">
                                        <img style={{ marginRight:'15px' }} src="/images/delete.png" alt="Delete" className="icon" />
                                        <div
                                            className={`toggle-button ${item.isScanning ? 'on' : ''}`}
                                            onClick={() => this.toggleValue(item.id)}>
                                            {item.isScanning ? 'on' : 'off'}
                                        </div>
                                    </div>
                                </div>
                            ))
                        ) : (
                            DIData.map(item => (
                                <div key={item.id} className="output-tag">
                                    <div className="tag-description">
                                        <h6 style={{ float: 'left' }}>{item.tagName}</h6>
                                        <p className="attribute" style={{ margin: "0px" }}>Description: {item.description}</p>
                                        <p className="attribute" style={{ margin: "0px" }}>Address: {item.address}</p>
                                        <p className="attribute">Scan Time: {item.scanTime} ms</p>
                                    </div>
                                    <p className="value">{item.driver === 0 ? 'SIMULATION' : 'RTU'}</p>
                                    <div className="edit-delete-icons">
                                        <img style={{ marginRight: '15px' }} src="/images/delete.png" alt="Delete" className="icon" />
                                        <div
                                            className={`toggle-button ${item.isScanning ? 'on' : ''}`}
                                            onClick={() => this.toggleValue(item.id)}>
                                            {item.isScanning ? 'on' : 'off'}
                                        </div>
                                    </div>
                                </div>
                            ))
                        )}
                    </div>
                </div>
            </div>
        );
    }
}