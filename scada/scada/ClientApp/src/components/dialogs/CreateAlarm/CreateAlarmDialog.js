import './CreateAlarmDialog.css';

const CreateAlarmDialog = ({ tagId, onClose }) => {
    console.log("openCreateDialog called");

    return (
        <div className="alarms-dialog">
            <p id="title">Create Alarm</p>
            <div id="alarms-list">
                <p
                    className="label">Type</p>
                <select
                    className="input">
                    <option value="LOW">Low limit</option>
                    <option value="HIGH">High limit</option>
                </select>
                <p
                    className="label">Limit</p>
                <input
                    className="input"
                    type="number"
                    name="low-limit"
                    maxLength="10"
                    placeholder="Type limit here..."
                ></input>
                <p
                    className="label">Priority</p>
                <select
                    className="input">
                    <option value="1">1 - low</option>
                    <option value="2">2 - medium</option>
                    <option value="3">3 - high</option>
                </select>
            </div>
            <div className="dialogs-button top-margin" id="bottom">
                <button className="btn">ADD</button>
                <button className="btn right-btn" onClick={onClose}>CLOSE</button>
            </div>
        </div>
    );
}

export default CreateAlarmDialog;