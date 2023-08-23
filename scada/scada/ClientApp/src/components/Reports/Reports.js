import React, { Component } from 'react';
import { NavMenu } from '../Nav/NavMenu';
import './Reports.css';
import '../../fonts.css';


export class Reports extends Component {
    static displayName = Reports.name;

    constructor(props) {
        super(props);
        this.state = { data: [], loading: true };
    }

    componentDidMount() {
        this.populateData();
    }

    static renderTable1(data) {
        return (
            <table className="table table-striped" aria-labelledby="tableLabel">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Type</th>
                        <th>Description</th>
                        <th>Scan Time</th>
                        <th>Scan</th>
                        <th>Low Limit</th>
                        <th>High Limit</th>
                        <th>Value</th>
                        <th>ALARM</th>
                    </tr>
                </thead>
                <tbody>
                    {data.map(item =>
                        <tr key={item.date}>
                            <td>{item.date}</td>
                            <td>{item.temperatureC}</td>
                            <td>{item.temperatureF}</td>
                            <td>{item.summary}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Reports.renderTable1(this.state.data);

        return (
            <div>
                <NavMenu showNavbar={true} />
                <h1 id="tableLabel">Reports</h1>
                {contents}
            </div>
        );
    }

    async populateData() {
        //todo change with data received from back
        const response = await fetch('weatherforecast');
        const data = await response.json();
        this.setState({ data: data, loading: false });
    }
}
