import { Component } from "react";
import axios from 'axios';
import '../../fonts.css';
import AITag from "../dialogs/AITag/AITag";
import AOTag from "../dialogs/AOTag/AOTag";
import { NavMenu } from "../Nav/NavMenu";
import './DatabaseManager.css';


export class DatabaseManager extends Component {

    constructor(props) {
        super(props);
        this.state = {
            isDropdownOpen: false,
            selectedItem: null,
            data: [],
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

    componentDidMount() {
        // Poziv API-ja nakon što se komponenta montira
        axios.get('http://localhost:5083/api/tag')
            .then(response => {
                this.setState({ data: response.data }); // Postavi dobijene podatke u stanje
                console.log(response.data);
            })
            .catch(error => {
                console.error('Error fetching data:', error);
            });
    }


    render() {
        const { isDropdownOpen, selectedItem, data, isDO } = this.state;

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

                <div id="output-tags">
                    <h3 style={{ margin: '15px' }}>Outputs</h3>
                    <div className={`toggle-switch ${isDO ? 'on' : ''}`} onClick={this.toggle}>
                        <div className="toggle-slider"></div>
                        <div className={`toggle-text digital ${isDO ? '' : 'active'}`}>Digital</div>
                        <div className={`toggle-text analog ${isDO ? 'active' : ''}`}>Analog</div>
                    </div>
                    {data.map(item => (
                        <div id='output-tag' key={item.id}>
                            <h6>{item.tagName}</h6>
                            <p>{item.description}</p>
                        </div>
                    ))} 
                </div>

                <div id="input-tags">
                    <h3 style={{ margin: '15px' }}>Inputs</h3>
                    {data.map(item => (
                        <div id='input-tag' key={item.id}>
                            <h6>{item.tagName}</h6>
                            <p>{item.description}</p>
                        </div>
                    ))}
                </div>
            </div>
        );
    }

}

