import './Alarms.css';

const Alarms = ({ onClose }) => {

    return (
        <div className="alarms-dialog">
            <p id="title">Alarms</p>
            {/* TODO : add here list of added alarms*/}
            <div id="alarms-list">
                <p>Here is the list of alarms</p>
            </div>
            <div className="dialogs-button" id="bottom">
                <button className="btn">ADD</button>
                <button className="btn right-btn" onClick={onClose}>CLOSE</button>
            </div>
        </div>
    );
};

export default Alarms;
