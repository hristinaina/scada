import { Component } from "react";


const AITag = ({ onClose }) => {
    return (
        <div className="dialog">
            <p>This is the dialog AITag</p>
            <button onClick={onClose}>Close</button>
        </div>
    );
};

export default AITag;