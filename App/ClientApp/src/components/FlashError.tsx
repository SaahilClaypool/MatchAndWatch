import React from "react";


export interface ErrorResponse {
    errors: InputError[]
}
export interface InputError {
    errorMessage: string,
    propertyName: string
}

export function FlashError(props: ErrorResponse) {
    return (
        <div className="error">
            <h3>
                Error
            </h3>
            <ul>
                {
                    props.errors.map(error => (
                        <li>
                            <strong>{error.propertyName}:</strong>
                            {error.errorMessage}
                        </li>
                    ))
                }
            </ul>
        </div >
    )
}