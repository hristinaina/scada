import { Component } from "react";
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
        };
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

    render() {
        const { isDropdownOpen, selectedItem } = this.state;
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

                {/* Dialogs */}
                {selectedItem === "AI" && <AITag onClose={this.closeDialog} />}
                {selectedItem === "AO" && <AOTag onClose={this.closeDialog} />}
            </div>
        );
    }

}

