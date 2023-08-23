import React, {useState, useEffect} from 'react';
import './Reports.css';
import '../../fonts.css';
import axios from 'axios';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';

export function FilterTableA({ onFilter }) {
    const [startDate, setStartDate] = useState(null);
    const [endDate, setEndDate] = useState(null);
    const [sortingType, setSortingType] = useState('time');

    const handleFilterClick = async () => {
        const filterOptions = {
            startDate,
            endDate,
            sortingType,
        };

        try {
            //1. real data:
           /* const response = await axios.post('/api/filter', filterOptions); // Modify the API endpoint as needed
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
            <h2>Date Filter</h2>
            <div>
                <label>Start date: </label>
                <DatePicker selected={startDate} onChange={date => setStartDate(date)} />
            </div>
            <div>
                <label>End date: </label>
                <DatePicker selected={endDate} onChange={date => setEndDate(date)} />
            </div>
            <div>
                <label>Sort by: </label>
                <select value={sortingType} onChange={e => setSortingType(e.target.value)}>
                    <option value="time">Time</option>
                    <option value="priority">Priority</option>
                </select>
            </div>
            <button onClick={handleFilterClick}>Apply Filter</button>
        </div>
    );
}

export default function TableA({ data }) {
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