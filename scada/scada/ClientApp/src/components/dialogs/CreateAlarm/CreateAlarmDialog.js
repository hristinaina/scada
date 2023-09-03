import './CreateAlarmDialog.css';

const CreateAlarmDialog = ({ tagId, onClose }) => {
    console.log("openCreateDialog called");

    return (
        <div className="alarms-dialog">
            <p id="title">Create Alarm</p>
            <div id="alarms-list">
                <p>Create alarm form...</p>
            </div>
            <div className="dialogs-button" id="bottom">
                <button className="btn">ADD</button>
                <button className="btn right-btn" onClick={onClose}>CLOSE</button>
            </div>
        </div>
    );
}

export default CreateAlarmDialog;