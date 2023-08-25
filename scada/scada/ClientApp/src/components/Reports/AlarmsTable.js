import React, {useState, useEffect} from 'react';
import './Reports.css';
import '../../fonts.css';
import axios from 'axios';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';

export function FilterAlarmTime({ onFilter }) {
    const [startDate, setStartDate] = useState(null);
    const [endDate, setEndDate] = useState(null);
    const [sortingType, setSortingType] = useState('time');

    const handleFilterClick = async (flag) => {
        try {
            if (flag === "load") {
                const startDate = new Date("11/14/2022 11:00:00 PM");
                const endDate = new Date("11/14/2024 11:00:00 PM");
                const filterOptions = {
                    startDate,
                    endDate,
                    sortingType,
                };
                const response = await axios.put('http://localhost:5083/api/report/alarmTime', filterOptions);
                onFilter(response.data); 
            }
            else {
                const filterOptions = {
                    startDate,
                    endDate,
                    sortingType,
                };
                const response = await axios.put('http://localhost:5083/api/report/alarmTime', filterOptions);
                onFilter(response.data); 

            }
        } catch (error) {
        console.error('Error fetching filtered data:', error);
        onFilter([]);
        }
    };

    useEffect(() => {
        setStartDate(new Date("11/14/2022 11:00:00 PM"));
        setEndDate(new Date("11/14/2024 11:00:00 PM"));
        handleFilterClick("load"); // Call the function on page load
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
            <div className="inline">
                <label className="labelR">Sort by: </label>
                <select className="inputR" value={sortingType} onChange={e => setSortingType(e.target.value)}>
                    <option value="time">Time</option>
                    <option value="priority">Priority</option>
                </select>
            </div>
            <button id="filterButton" onClick={handleFilterClick}>Apply Filter</button>
        </div>
    );
}

export function FilterAlarmPriority({ onFilter }) {
    const [priority, setPriority] = useState('');

    const handleFilterClick = async () => {
        try {
            const response = await axios.post('/api/report/alarmPriority', { priority });
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
                <label className="labelR">Priority: </label>
                <input className="inputOne"
                    type="text"
                    value={priority}
                    onChange={e => setPriority(e.target.value)}
                />
            </div>
            <button id="filterButton" onClick={handleFilterClick}>Apply Filter</button>
        </div>
    );
}

export default function AlarmsTable({ data }) {
    return (
        <table className="table table-striped" aria-labelledby="tableLabel">
            <thead>
                <tr>
                    <th>Alarm Id</th>
                    <th>Type</th>
                    <th>Limit</th>
                    <th>Priority</th>
                    <th>Date</th>
                    <th>Tag name</th>
                </tr>
            </thead>
            <tbody>
                {data.map((item, index) => (
                    <tr key={index}>
                        <td>{item.alarmId}</td>
                        <td>{item.type}</td>
                        <td>{item.limit}</td>
                        <td>{item.priority}</td>
                        <td>{item.date}</td>
                        <td>{item.tagName}</td>
                    </tr>
                ))}
            </tbody>
        </table>
    );
}