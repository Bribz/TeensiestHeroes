using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0.3,0.25]")]
	public partial class PlayerNetworkObject : NetworkObject
	{
		public const int IDENTITY = 3;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		private Vector3 _mPosition;
		public event FieldEvent<Vector3> mPositionChanged;
		public InterpolateVector3 mPositionInterpolation = new InterpolateVector3() { LerpT = 0.3f, Enabled = true };
		public Vector3 mPosition
		{
			get { return _mPosition; }
			set
			{
				// Don't do anything if the value is the same
				if (_mPosition == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x1;
				_mPosition = value;
				hasDirtyFields = true;
			}
		}

		public void SetmPositionDirty()
		{
			_dirtyFields[0] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_mPosition(ulong timestep)
		{
			if (mPositionChanged != null) mPositionChanged(_mPosition, timestep);
			if (fieldAltered != null) fieldAltered("mPosition", _mPosition, timestep);
		}
		private Quaternion _mRotation;
		public event FieldEvent<Quaternion> mRotationChanged;
		public InterpolateQuaternion mRotationInterpolation = new InterpolateQuaternion() { LerpT = 0.25f, Enabled = true };
		public Quaternion mRotation
		{
			get { return _mRotation; }
			set
			{
				// Don't do anything if the value is the same
				if (_mRotation == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x2;
				_mRotation = value;
				hasDirtyFields = true;
			}
		}

		public void SetmRotationDirty()
		{
			_dirtyFields[0] |= 0x2;
			hasDirtyFields = true;
		}

		private void RunChange_mRotation(ulong timestep)
		{
			if (mRotationChanged != null) mRotationChanged(_mRotation, timestep);
			if (fieldAltered != null) fieldAltered("mRotation", _mRotation, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			mPositionInterpolation.current = mPositionInterpolation.target;
			mRotationInterpolation.current = mRotationInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _mPosition);
			UnityObjectMapper.Instance.MapBytes(data, _mRotation);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_mPosition = UnityObjectMapper.Instance.Map<Vector3>(payload);
			mPositionInterpolation.current = _mPosition;
			mPositionInterpolation.target = _mPosition;
			RunChange_mPosition(timestep);
			_mRotation = UnityObjectMapper.Instance.Map<Quaternion>(payload);
			mRotationInterpolation.current = _mRotation;
			mRotationInterpolation.target = _mRotation;
			RunChange_mRotation(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _mPosition);
			if ((0x2 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _mRotation);

			return dirtyFieldsData;
		}

		protected override void ReadDirtyFields(BMSByte data, ulong timestep)
		{
			if (readDirtyFlags == null)
				Initialize();

			Buffer.BlockCopy(data.byteArr, data.StartIndex(), readDirtyFlags, 0, readDirtyFlags.Length);
			data.MoveStartIndex(readDirtyFlags.Length);

			if ((0x1 & readDirtyFlags[0]) != 0)
			{
				if (mPositionInterpolation.Enabled)
				{
					mPositionInterpolation.target = UnityObjectMapper.Instance.Map<Vector3>(data);
					mPositionInterpolation.Timestep = timestep;
				}
				else
				{
					_mPosition = UnityObjectMapper.Instance.Map<Vector3>(data);
					RunChange_mPosition(timestep);
				}
			}
			if ((0x2 & readDirtyFlags[0]) != 0)
			{
				if (mRotationInterpolation.Enabled)
				{
					mRotationInterpolation.target = UnityObjectMapper.Instance.Map<Quaternion>(data);
					mRotationInterpolation.Timestep = timestep;
				}
				else
				{
					_mRotation = UnityObjectMapper.Instance.Map<Quaternion>(data);
					RunChange_mRotation(timestep);
				}
			}
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

			if (mPositionInterpolation.Enabled && !mPositionInterpolation.current.Near(mPositionInterpolation.target, 0.0015f))
			{
				_mPosition = (Vector3)mPositionInterpolation.Interpolate();
				RunChange_mPosition(mPositionInterpolation.Timestep);
			}
			if (mRotationInterpolation.Enabled && !mRotationInterpolation.current.Near(mRotationInterpolation.target, 0.0015f))
			{
				_mRotation = (Quaternion)mRotationInterpolation.Interpolate();
				RunChange_mRotation(mRotationInterpolation.Timestep);
			}
		}

		private void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[1];

		}

		public PlayerNetworkObject() : base() { Initialize(); }
		public PlayerNetworkObject(NetWorker networker, INetworkBehavior networkBehavior = null, int createCode = 0, byte[] metadata = null) : base(networker, networkBehavior, createCode, metadata) { Initialize(); }
		public PlayerNetworkObject(NetWorker networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}