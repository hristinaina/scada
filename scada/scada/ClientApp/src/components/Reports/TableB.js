import React, { useState, useEffect } from 'react';
import './Reports.css';
import '../../fonts.css';
import axios from 'axios';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';

export function FilterTableB({ onFilter }) {
    const [priority, setPriority] = useState('');

    const handleFilterClick = async () => {
        try {
            //1. real data
            /*const response = await axios.post('/api/filter', { priority });
            onFilter(response.data); // Pass the filtered data to the parent component*/
            //2. test data:
            const response = await fetch('weatherforecast');
            const data = await response.json();
            onFilter(data);
        } catch (error) {
            console.error('Error fetching filtered data:', error);
        }
    };

    useEffect(() => {
        handleFilterClick(); // Call the function on page load
    }, []); // Empty dependency array means it runs only on mount

    return (
        <div>
            <div>
                <label>Priority: </label>
                <input
                    type="text"
                    value={priority}
                    onChange={e => setPriority(e.target.value)}
                />
            </div>
            <button onClick={handleFilterClick}>Apply Filter</button>
        </div>
    );
}


export default function TableB({ data }) {
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