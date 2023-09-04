import './Alarms.css';
import React, { useEffect, useState } from 'react';
import axios from 'axios';


const Alarms = ({ onClose, openCreateAlarmDialog, tagId }) => {
    const [alarms, setAlarms] = useState([]);

    useEffect(() => {
        async function fetchData() {
            try {
                const response = await axios.get('http://localhost:5083/api/tag/alarm/' + tagId);
                setAlarms(response.data);
            } catch (error) {
                console.error('Error fetching data:', error);
            }
        }

        fetchData();
    }, [alarms]);

    return (
        <div className="alarms-dialog">
            <p id="title">Alarms</p>
            {/* TODO : add here list of added alarms*/}
            <div id="alarms-list">
                {alarms.map((item) => (
                    <div>
                        <p className="alarm-title">ALARM</p>
                        <p className="alarms-label">TYPE</p>
                        <p className="alarms-value">{item.type === 1 ? "low" : "high"}</p>
                        <p className="alarms-label">PRIORITY</p>
                        <p className="alarms-value">{item.priority}</p>
                        <p className="alarms-label">LIMIT</p>
                        <p className="alarms-value">{item.limit}</p>
                    </div>
                ))}
            </div>
            <div className="dialogs-button" id="bottom">
                <button className="btn" onClick={openCreateAlarmDialog}>ADD</button>
                <button className="btn right-btn" onClick={onClose}>CLOSE</button>
            </div>
        </div>
    );
};

export default Alarms;
