import './Alarms.css';
import React, { useEffect, useState } from 'react';
import axios from 'axios';


const Alarms = ({ onClose, openCreateAlarmDialog, tagId }) => {
    const [alarms, setAlarms] = useState([]);
    const [showDeleteDialog, setShowDeleteDialog] = useState(false);
    const [id, setId] = useState(-1);

    const fetchData = async () => {
        try {
            const response = await axios.get('http://localhost:5083/api/tag/alarm/' + tagId);
            setAlarms(response.data);
        } catch (error) {
            console.error('Error fetching data:', error);
        }
    };

    useEffect(() => {
        fetchData();
    }, [tagId]); // fetch data whenever tagId changes

    const openDeleteDialog = (id) => {
        setShowDeleteDialog(true);
        setId(id);
    }

    const closeDeleteDialog = () => {
        setShowDeleteDialog(false);
        setId(-1);
    }

    const deleteAlarm = async () => {
        try {
            await axios.delete('http://localhost:5083/api/tag/alarm/' + id);
            console.log("Successfully deleted!");
            fetchData();
        }
        catch (error) {
            console.log("Error ocurred: ", error);
        }

        closeDeleteDialog();
    };

    return (
        <div>
            <div className="alarms-dialog">
                <p id="title">Alarms</p>
                {alarms.length === 0 ? (
                    <p id="no-added-alarms">No added alarms</p>
                ) : (
                    <div id="alarms-list">

                        {alarms.map((item) => (
                            <div key={item.id}>
                                <p className="alarm-title">ALARM</p>
                                <img src="/images/delete.png" alt="Delete" className="icon" onClick={() => openDeleteDialog(item.id)} />
                                <p className="alarms-label">TYPE</p>
                                <p className="alarms-value">{item.type === 1 ? "low" : "high"}</p>
                                <p className="alarms-label">PRIORITY</p>
                                <p className="alarms-value">{item.priority}</p>
                                <p className="alarms-label">LIMIT</p>
                                <p className="alarms-value">{item.limit}</p>
                            </div>
                        ))}
                    </div>
                )}
                <div className="dialogs-button" id="bottom">
                    <button className="btn" onClick={openCreateAlarmDialog}>ADD</button>
                    <button className="btn right-btn" onClick={onClose}>CLOSE</button>
                </div>
            
            </div>

            {showDeleteDialog && (
                <div className="dialog-container">
                    <div className="delete-dialog">
                        <h2 className="dialog-title">Delete</h2>
                        <p className="dialog-message">Are you sure you want to delete this alarm?</p>
                        <div className="dialog-buttons">
                            <button className="delete-button button" onClick={deleteAlarm}>Delete</button>
                            <button className="close-button button" onClick={closeDeleteDialog}>Close</button>
                        </div>
                    </div>
                </div>
            )}
        </div>


    );
};

export default Alarms;
