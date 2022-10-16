import React from 'react';
import { ChatBoxProps } from '../contracts';
import { ChatBoxContent } from '../ChatBoxContent';
import { ChatField } from '../ChatField';

export const ChatBox : React.FC<ChatBoxProps> = ({ label, author }) => {
    return (
      <div className="ChatBoxContainer">
        <label>{label}</label>
        <ChatBoxContent author={author} />
        <ChatField author={author} />
      </div>
    );
  }

export default ChatBox;
ChatBox.displayName = 'ChatBox';
