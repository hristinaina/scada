import React, { useState, useEffect } from 'react';
import './Reports.css';
import '../../fonts.css';
import axios from 'axios';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';

export function FilterTagTime({ onFilter }) {
    const [startDate, setStartDate] = useState(null);
    const [endDate, setEndDate] = useState(null);

    const handleFilterClick = async () => {
        const filterOptions = {
            startDate,
            endDate,
        };

        try {
             const response = await axios.post('/api/report/tagTime', filterOptions); 
             onFilter(response.data); // Pass the filtered data to the parent component
        } catch (error) {
            console.error('Error fetching filtered data:', error);
        }
    };

    useEffect(() => {
        handleFilterClick(); // Call the function on page load
    }, []); // Empty dependency array means it runs only on mount

    return (
        <div className="filterContainer">
            <div className="inline">
                <label className="labelR">Start date: </label>
                <DatePicker className="inputR" selected={startDate} onChange={date => setStartDate(date)} />
            </div>
            <div className="inline">
                <label className="labelR">End date: </label>
                <DatePicker className="inputR" selected={endDate} onChange={date => setEndDate(date)} />
            </div>
            <button id="filterButton" onClick={handleFilterClick}>Apply Filter</button>
        </div>
    );
}

export function FilterTagId({ onFilter }) {
    const [identifier, setIdentifier] = useState('');

    const handleFilterClick = async () => {
        try {
            const response = await axios.post('/api/report/tagId', { identifier });
            onFilter(response.data); // Pass the filtered data to the parent component
        } catch (error) {
            console.error('Error fetching filtered data:', error);
        }
    };

    useEffect(() => {
        handleFilterClick(); 
    }, []); 

    return (
        <div className="filterContainer">
            <div>
                <label className="labelR">Tag id: </label>
                <input className="inputOne"
                    type="text"
                    value={identifier}
                    onChange={e => setIdentifier(e.target.value)}
                />
            </div>
            <button id="filterButton" onClick={handleFilterClick}>Apply Filter</button>
        </div>
    );
}

export function FilterInputTag({ filterProps }) {
    const { onFilter, type } = filterProps;

    const handleFilterClick = async () => {
        try {
            if (type === "AI") {
                const response = await axios.post('/api/report/tagAI');
                onFilter(response.data); 
            }
            else if (type === "DI") {
                const response = await axios.post('/api/report/tagDI');
                onFilter(response.data);
            }
        } catch (error) {
            console.error('Error fetching filtered data:', error);
        }
    };

    useEffect(() => {
        handleFilterClick(); // Call the function on page load
    }, [type]); // it runs also whenever type is changed 

    return (
        <div className="filterContainer">
        </div>
    );
}

export default function TagsTable({ data }) {
    return (
        <table className="table table-striped" aria-labelledby="tableLabel">
            <thead>
                <tr>
                    <th>Tag Id</th>
                    <th>Name</th>
                    <th>Type</th>
                    <th>Value</th>
                    <th>Units</th>
                    <th>Date</th>
                </tr>
            </thead>
            <tbody>
                {data.map((item, index) => (
                    <tr key={index}>
                        <td>{item.temperatureF}</td>
                        <td>{item.temperatureF}</td>
                        <td>{item.temperatureC}</td>
                        <td>{item.temperatureC}</td>
                        <td></td>
                        <td>{item.date}</td>
                    </tr>
                ))}
            </tbody>
        </table>
    );
}