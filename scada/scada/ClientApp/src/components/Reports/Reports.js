import React, { Component, useState, useEffect } from 'react';
import { NavMenu } from '../Nav/NavMenu';
import axios from 'axios';
import './Reports.css';
import '../../fonts.css';

export default function Reports() {
    const [selectedTable, setSelectedTable] = useState('Table A');
    const [tableData, setTableData] = useState([]);

    useEffect(() => {
        fetchData();
    }, [selectedTable]);

    const fetchData = async () => {
        try {
            //1. real data:
            //const response = await axios.get(`/api/${selectedTable.toLowerCase()}`);
            //setTableData(response.data);
            //2. test data:
            const response = await fetch('weatherforecast');
            const data = await response.json();
            setTableData(data);
        } catch (error) {
            console.error('Error fetching data:', error);
        }
    };

    const renderTable = () => {
        if (selectedTable === 'Table A') {
            return <TableA data={tableData} />;
        } else if (selectedTable === 'Table B') {
            return <TableB data={tableData} />;
        }
    };

    return (
        <div>
            <NavMenu showNavbar={true}></NavMenu>
            <h1>Dynamic Table Example</h1>
            <Dropdown onSelect={setSelectedTable} />
            {renderTable()}
        </div>
    );
}

function Dropdown({ onSelect }) {
    const options = ['Table A', 'Table B'];
    const [selectedOption, setSelectedOption] = useState(options[0]);

    const handleSelect = (event) => {
        setSelectedOption(event.target.value);
        onSelect(event.target.value);
    };

    return (
        <div>
            <select value={selectedOption} onChange={handleSelect}>
                {options.map((option, index) => (
                    <option key={index} value={option}>
                        {option}
                    </option>
                ))}
            </select>
        </div>
    );
}

function TableA({ data }) {
    return (
        <table>
            <thead>
                <tr>
                    <th>Column A</th>
                    <th>Column B</th>
                </tr>
            </thead>
            <tbody>
                {data.map((item, index) => (
                    <tr key={index}>
                        <td>{item.date}</td>
                        <td>{item.temperatureF}</td>
                    </tr>
                ))}
            </tbody>
        </table>
    );
}

function TableB({ data }) {
    return (
        <table>
            <thead>
                <tr>
                    <th>Column X</th>
                    <th>Column Y</th>
                    <th>Column Z</th>
                </tr>
            </thead>
            <tbody>
                {data.map((item, index) => (
                    <tr key={index}>
                        <td>{item.date}</td>
                        <td>{item.temperatureF}</td>
                        <td>{item.temperatureC}</td>
                    </tr>
                ))}
            </tbody>
        </table>
    );
}
