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
            isDO: false,
            isDI: true,
        };
    }

    toggle = () => {
        this.setState(prevState => ({
            isDO: !prevState.isDO, // Promeni stanje na suprotno
        }));
    };

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
        const AOData = await TagService.getAOData(); // Zamisljena metoda za dohvatanje analognih podataka
        const DOData = await TagService.getDOData(); // Zamisljena metoda za dohvatanje digitalnih podataka
        this.setState({ AOData, DOData });
    }


    render() {
        const { isDropdownOpen, selectedItem, data, isDO, AOData, DOData } = this.state;

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
                        <p style={{ margin: '0px', fontSize: '26px' }}>Outputs</p>
                        <div className={`toggle-switch ${isDO ? 'on' : ''}`} onClick={this.toggle}>
                            <div className="toggle-slider"></div>
                            <div className={`toggle-text digital ${isDO ? '' : 'active'}`}>Digital</div>
                            <div className={`toggle-text analog ${isDO ? 'active' : ''}`}>Analog</div>
                        </div>
                    </div>
                    <div className="object-list">
                        {isDO ? (
                            AOData.map(item => (
                                <div key={item.id} className="output-tag">
                                    <h6>{item.tagName}</h6>
                                    {/*<p>Attribute 1: {item.attribute1}</p>*/}
                                    {/*<p>Attribute 2: {item.attribute2}</p>*/}
                                    {/* Dodajte više atributa po potrebi */}
                                </div>
                            ))
                        ) : (
                            DOData.map(item => (
                                <div key={item.id} className="output-tag">
                                    <div className="tag-description">
                                        <h6 style={{ float: 'left' }}>{item.tagName}</h6>
                                        <p className="description">Description: {item.description}</p>
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

                {/*<div id="input-tags">*/}
                {/*    <h3 style={{ margin: '15px' }}>Inputs</h3>*/}
                {/*    {data.map(item => (*/}
                {/*        <div id='input-tag' key={item.id}>*/}
                {/*            <h6>{item.tagName}</h6>*/}
                {/*            <p>{item.description}</p>*/}
                {/*        </div>*/}
                {/*    ))}*/}
                {/*</div>*/}
            </div>
        );
    }

}

