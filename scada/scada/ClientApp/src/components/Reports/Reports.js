import React, { useState } from 'react';
import { NavMenu } from '../Nav/NavMenu';
import './Reports.css';
import '../../fonts.css';
import axios from 'axios';
import TableA, { FilterTableA } from './TableA';
import TableB, { FilterTableB } from './TableB';

export default function Reports() {
    const [selectedTable, setSelectedTable] = useState('Table A');
    const [tableData, setTableData] = useState([]);

    const handleFilter = data => {
        setTableData(data); // Update filtered data based on the filter API response
    };

    const renderTable = () => {
        if (selectedTable === 'Table A') {
            return <div><FilterTableA onFilter={handleFilter} />
                        <TableA data={tableData} />
                   </div>;
        } else if (selectedTable === 'Table B') {
            return <div><FilterTableB onFilter={handleFilter} />
                        <TableB data={tableData} />
                   </div>;
        }
    };

    return (
        <div>
            <NavMenu showNavbar={true}></NavMenu>
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

