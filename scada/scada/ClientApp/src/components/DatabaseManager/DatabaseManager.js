import { Component } from "react";
import '../../fonts.css';
import { NavMenu } from "../Nav/NavMenu";
import './DatabaseManager.css';


export class DatabaseManager extends Component {

    constructor(props) {
        super(props);
        this.state = {
            isDropdownOpen: false, // Initialize dropdown state as closed
        };
    }

    toggleDropdown = () => {
        this.setState((prevState) => ({
            isDropdownOpen: !prevState.isDropdownOpen,
        }));
    };

    render() {
        const { isDropdownOpen } = this.state;
        return (
            <div>
                <NavMenu showNavbar={true} />
                <h1 id="tableLabel">Database Manager</h1>
                {/*<img alt="." src="../..images/plus.png"/>*/}
                <p id="add-tag" onClick={this.toggleDropdown}>Add tag</p>

                {isDropdownOpen && (
                    <div id="dropdown">
                        <ul id="dropdown-tags">
                            <li className="tag bottom-margin">ANALOG OUTPUT</li>
                            <li className="tag bottom-margin">ANALOG INPUT</li>
                            <li className="tag bottom-margin">DIGITAL OUTPUT</li>
                            <li className="tag">DIGITAL INPUT</li>
                        </ul>
                    </div>
                )}
            </div>
        );
    }

}

