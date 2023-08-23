import { Component } from "react";
import '../../fonts.css';
import { NavMenu } from "../Nav/NavMenu";
import './DatabaseManager.css';


export class DatabaseManager extends Component {

    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div>
                <NavMenu showNavbar={true} />
                <h1 id="tableLabel">Database Manager</h1>
                {/*<img alt="." src="../..images/plus.png"/>*/}
                <p id="add-tag">Add tag</p>
            </div>
        );
    }

}

