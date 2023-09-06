import './CreateAlarmDialog.css';
import React, { useState } from 'react';
import axios from 'axios';

const CreateAlarmDialog = ({ tagId, onClose }) => {
    const [selectedPriority, setSelectedPriority] = useState('1');
    const [selectedType, setSelectedType] = useState('LOW');

    const create = async () => {
        const type = selectedType;
        const priority = selectedPriority;
        const limit = document.getElementsByName("limit")[0].value;

        const alarm = {
            Type: type,
            Priority: parseInt(priority),
            Limit: parseFloat(limit),
            TagId: tagId 
        }

        try { 
            const response = await axios.post('http://localhost:5083/api/tag/alarm', alarm);
            console.log("Alarm added successfully!", response.data);
        }
        catch (error) {
            console.log("Add failed: ", error);
        }
        onClose();

    }

    return (
        <div className="alarms-dialog">
            <p id="title">Create Alarm</p>
            <div id="alarms-list">
                <p
                    className="label">Type</p>
                <select
                    className="input"
                    value={selectedType}
                    onChange={(e) => setSelectedType(e.target.value)}                >
                    <option value="LOW">Low limit</option>
                    <option value="HIGH">High limit</option>
                </select>
                <p
                    className="label">Limit</p>
                <input
                    className="input"
                    type="number"
                    name="limit"
                    maxLength="10"
                    placeholder="Type limit here..."
                ></input>
                <p
                    className="label">Priority</p>
                <select
                    className="input"
                    value={selectedPriority}
                    onChange={(e) => setSelectedPriority(e.target.value)}                >
                    <option value="1">1 - low</option>
                    <option value="2">2 - medium</option>
                    <option value="3">3 - high</option>
                </select>
            </div>
            <div className="dialogs-button top-margin" id="bottom">
                <button className="btn" onClick={create}>ADD</button>
                <button className="btn right-btn" onClick={onClose}>CLOSE</button>
            </div>
        </div>
    );
}

export default CreateAlarmDialog;