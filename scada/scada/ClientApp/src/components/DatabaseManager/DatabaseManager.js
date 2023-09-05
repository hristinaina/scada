import React, { Component } from "react";
import '../../fonts.css';
import AITag from "../dialogs/AITag/AITag";
import AOTag from "../dialogs/AOTag/AOTag";
import DITag from "../dialogs/DITag/DITag";
import DOTag from "../dialogs/DOTag/DOTag";
import { NavMenu } from "../Nav/NavMenu";
import './DatabaseManager.css';
import TagService from "../../services/TagService";
import axios from 'axios';


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
            showDeleteDialog: false,
            showAnalogEditDialog: false,
            showDigitalEditDialog: false,
            editValue: '',
            chosenTag: -1,
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

    toggleValue = async(item) => {
        const newItem = { ...item };
        newItem.isScanning = !newItem.isScanning;

        this.setState(prevState => ({
            DIData: prevState.DIData.map(dataItem => {
                if (dataItem.id === newItem.id) {
                    return newItem;
                }
                return dataItem;
            }),
            AIData: prevState.AIData.map(dataItem => {
                if (dataItem.id === newItem.id) {
                    return newItem;
                }
                return dataItem;
            })
        }));

        try {
            await axios.put('http://localhost:5083/api/tag/scan' + item.id);
            console.log("Successfully changed scan on/off!");
        }
        catch (error) {
            console.log("Error ocurred: ", error);
        }
    }

    toggleDropdown = () => {
        this.setState((prevState) => ({
            isDropdownOpen: !prevState.isDropdownOpen,
        }));
    };

    openDialog = (selectedItem) => {
        this.setState({
            selectedItem: selectedItem,
            isDropdownOpen: false,
        });
    };

    closeDialog = () => {
        this.setState({
            selectedItem: null,
        });
        this.componentDidMount();
    };

    openDeleteDialog = (id) => {
        this.setState({
            showDeleteDialog: true,
            chosenTag: id,
        });
    }

    closeDeleteDialog = () => {
        this.setState({
            showDeleteDialog: false,
        });
    }

    delete = async () => {
        console.log("You clicked: " + this.state.chosenTag);
        try {
            await axios.delete('http://localhost:5083/api/tag/' + this.state.chosenTag);
            console.log("Successfully deleted!");
        }
        catch (error) {
            console.log("Error ocurred: ", error);
        }
        this.closeDeleteDialog();
        this.componentDidMount();
    }

    openDigitalEditDialog = (item) => {
        this.setState({
            showDigitalEditDialog: true,
            chosenTag: item.id,
            editValue: item.value,
        });
    }

    openAnalogEditDialog = (item) => {
        this.setState({
            showAnalogEditDialog: true,
            chosenTag: item.id,
            editValue: item.value,
        });
    }

    handleEditedValueChange = (event) => {
        this.setState({ editValue: event.target.value });
    };

    saveEditedValue = () => {
        this.setState({
            showDigitalEditDialog: false,
            showAnalogEditDialog: false
        });
        //todo send api
    };

    closeEditDialog = () => {
        this.setState({
            showDigitalEditDialog: false,
            showAnalogEditDialog: false
        });
    }

    async componentDidMount() {
        const AOData = await TagService.getAOData(); 
        const DOData = await TagService.getDOData(); 
        const AIData = await TagService.getAIData();
        const DIData = await TagService.getDIData();
        console.log(DIData)
        this.setState({ AOData, DOData, AIData, DIData });
    }


    render() {
        const { isDropdownOpen, selectedItem, isDO, isDI, AOData, DOData, AIData, DIData, showDeleteDialog, showAnalogEditDialog, showDigitalEditDialog } = this.state;

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

                {showDeleteDialog && (
                    <div className="dialog-container">
                        <div className="delete-dialog">
                            <h2 className="dialog-title">Delete</h2>
                            <p className="dialog-message">Are you sure you want to delete this tag?</p>
                            <div className="dialog-buttons">
                                <button className="delete-button button" onClick={this.delete}>Delete</button>
                                <button className="close-button button" onClick={this.closeDeleteDialog}>Close</button>
                            </div>
                        </div>
                    </div>
                )}

                {showAnalogEditDialog && (
                    <div className="dialog-container">
                        <div className="delete-dialog">
                            <h2 className="dialog-title">Edit Value</h2>
                            <input className="input"
                                type="text"
                                value={this.state.editValue}
                                onChange={this.handleEditedValueChange}
                            />
                            <div id="buttons">
                                <button className="btn" id="save" onClick={this.saveEditedValue}>Save</button>
                                <button className="btn" onClick={this.closeEditDialog}>Close</button>
                            </div>
                        </div>
                    </div>
                )}

                {showDigitalEditDialog && (
                    <div className="dialog-container">
                        <div className="delete-dialog">
                            <h2 className="dialog-title">Edit Value</h2>
                            <select className="input"
                                value={this.state.editValue}
                                onChange={this.handleEditedValueChange}
                            >
                                <option value="0">0</option>
                                <option value="1">1</option>
                            </select>
                            <div id="buttons">
                                <button className="btn" id="save" onClick={this.saveEditedValue}>Save</button>
                                <button className="btn" onClick={this.closeEditDialog}>Close</button>
                            </div>
                        </div>
                    </div>
                )}

                {/*Dialogs */}
                {selectedItem === "AI" && <AITag onClose={this.closeDialog} />}
                {selectedItem === "AO" && <AOTag onClose={this.closeDialog} />}
                {selectedItem === "DI" && <DITag onClose={this.closeDialog} />}
                {selectedItem === "DO" && <DOTag onClose={this.closeDialog} />}

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
                                        <img src="/images/delete.png" alt="Delete" className="icon" onClick={() => this.openDeleteDialog(item.id)} />
                                        <img src="/images/pencil.png" alt="Edit" className="icon" onClick={() => this.openAnalogEditDialog(item)} />
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
                                        <img src="/images/delete.png" alt="Delete" className="icon" onClick={() => this.openDeleteDialog(item.id)} />
                                        <img src="/images/pencil.png" alt="Edit" className="icon" onClick={() => this.openDigitalEditDialog(item)} />
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
                                        <img style={{ marginRight: '15px' }} src="/images/delete.png" alt="Delete" className="icon" onClick={() => this.openDeleteDialog(item.id)} />
                                        <img style={{ marginRight: '15px', marginBottom: '2px', marginTop: '2px' }} src="/images/bell.png" alt="Alarm" className="icon" />
                                        <div
                                            className={`toggle-button ${item.isScanning ? 'on' : ''}`}
                                            onClick={() => this.toggleValue(item)}>
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
                                        <img style={{ marginRight: '15px' }} src="/images/delete.png" alt="Delete" className="icon" onClick={() => this.openDeleteDialog(item.id)} />
                                        <div
                                            className={`toggle-button ${item.isScanning ? 'on' : ''}`}
                                            onClick={() => this.toggleValue(item)}>
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