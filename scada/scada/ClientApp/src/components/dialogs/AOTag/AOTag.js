import { Component } from "react";


const AOTag = ({ onClose }) => {
    return (
        <div className="dialog">
            <p>This is the dialog for AOTag</p>
            <button onClick={onClose}>Close</button>
        </div>
    );
};

export default AOTag;