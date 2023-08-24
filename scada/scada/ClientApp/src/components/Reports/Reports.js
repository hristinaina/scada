import React, { useState } from 'react';
import { NavMenu } from '../Nav/NavMenu';
import './Reports.css';
import '../../fonts.css';
import axios from 'axios';
import AlarmsTable, { FilterAlarmTime, FilterAlarmPriority } from './AlarmsTable';
import TagsTable, { FilterTagTime, FilterTagId, FilterInputTag } from './TagsTable';

export default function Reports() {
    const [selectedTable, setSelectedTable] = useState('Show alarm history by date');
    const [tableData, setTableData] = useState([]);

    const handleFilter = data => {
        setTableData(data); // Update filtered data based on the filter API response
    };

    const renderTable = () => {
        if (selectedTable === 'Show alarm history by date') {
            return <div><FilterAlarmTime onFilter={handleFilter} />
                        <AlarmsTable data={tableData} />
                   </div>;
        } else if (selectedTable === 'Show alarm history by priority') {
            return <div><FilterAlarmPriority onFilter={handleFilter} />
                        <AlarmsTable data={tableData} />
                   </div>;
        } else if (selectedTable === 'Show tag history by date') {
            return <div><FilterTagTime onFilter={handleFilter} />
                <TagsTable data={tableData} />
            </div>;
        } else if (selectedTable === 'Show last value of all AI tags') {
            const filterProps = {
                onFilter: handleFilter,
                type: 'AI',
            };
            return <div><FilterInputTag filterProps={filterProps} />
                <TagsTable data={tableData} />
            </div>;
        }
        else if (selectedTable === 'Show last value of all DI tags') {
            const filterProps = {
                onFilter: handleFilter,
                type: 'DI',
            };
            return <div><FilterInputTag filterProps={filterProps} />
                <TagsTable data={tableData} />
            </div>;
        }
        else if (selectedTable === 'Show tag history by identifier') {
            return <div><FilterTagId onFilter={handleFilter} />
                <TagsTable data={tableData} />
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
    const options = ['Show alarm history by date', 'Show alarm history by priority', 'Show tag history by date',
        'Show last value of all AI tags', 'Show last value of all DI tags', 'Show tag history by identifier'];
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

