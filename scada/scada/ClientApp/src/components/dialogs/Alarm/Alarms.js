import CreateAlarmDialog from '../CreateAlarm/CreateAlarmDialog';
import './Alarms.css';
import React, { useState } from 'react';


const Alarms = ({ onClose, openCreateAlarmDialog }) => {
    const [isCreateDialogOpen, setCreateDialogOpen] = useState(false);
    const [selectedTagId, setSelectedTagId] = useState(null);

    
    const openCreateDialog = (tagId) => {
        console.log("tag id: ", tagId);
        setSelectedTagId(tagId);
        setCreateDialogOpen(true);
        onClose();
    }

    const closeCreateDialog = () => {
        setSelectedTagId(null);
        setCreateDialogOpen(false);
    }

    return (
        <div className="alarms-dialog">
            <p id="title">Alarms</p>
            {/* TODO : add here list of added alarms*/}
            <div id="alarms-list">
                <p>Here is the list of alarms</p>
            </div>
            <div className="dialogs-button" id="bottom">
                <button className="btn" onClick={openCreateAlarmDialog}>ADD</button>
                <button className="btn right-btn" onClick={onClose}>CLOSE</button>
            </div>

            {console.log("isCreateDialogOpen:", isCreateDialogOpen)} {/* Add this line */}
            {isCreateDialogOpen && (<CreateAlarmDialog onClose={closeCreateDialog} tagId={selectedTagId} />)}
        </div>
    );
};

export default Alarms;
